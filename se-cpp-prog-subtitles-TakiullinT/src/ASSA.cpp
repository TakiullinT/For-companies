#include "../include/ASSA.h"

const Vector< std::string >& ASSA::get_styles() const
{
	return styles;
}

long long ASSA::parsing_time(const std::string& time_str)
{
	int hours, minutes, seconds, centiseconds;
	if (std::sscanf(time_str.c_str(), "%d:%d:%d.%d", &hours, &minutes, &seconds, &centiseconds) != 4)
	{
		throw std::invalid_argument("Invalid time format");
	}

	if (hours < 0 || hours > 23 || minutes < 0 || minutes > 59 || seconds < 0 || seconds > 59 || centiseconds < 0 || centiseconds > 99)
	{
		throw std::invalid_argument("Time values out of range");
	}

	return ((hours * 3600 + minutes * 60 + seconds) * 1000) + (centiseconds * 10);
}

std::string ASSA::formatting_time(long long time_ms)
{
	int hours = time_ms / 3600000;
	time_ms %= 3600000;
	int minutes = time_ms / 60000;
	time_ms %= 60000;
	int seconds = time_ms / 1000;
	time_ms %= 1000;
	int centiseconds = time_ms / 10;

	char buffer[16];
	std::snprintf(buffer, sizeof(buffer), "%02d:%02d:%02d.%02d", hours, minutes, seconds, centiseconds);
	return std::string(buffer);
}

void ASSA::parse_script_info(const std::string& line)
{
	std::string raw_data = line.substr(line.find(':') + 1);
	Vector< std::string > fields;
	size_t start = 0, end;

	for (int i = 0; i < 8; ++i)
	{
		end = raw_data.find(',', start);
		if (end == std::string::npos)
		{
			throw std::runtime_error("Invalid Dialogue line format");
		}
		fields.push_back(raw_data.substr(start, end - start));
		start = end + 1;
	}
	std::string dialogue_text = raw_data.substr(start);

	Subtitle_data subtitle;
	subtitle.id = entries.get_size() + 1;
	subtitle.layer = std::stoi(fields[0]);
	subtitle.start_time = parsing_time(fields[1]);
	subtitle.end_time = parsing_time(fields[2]);
	subtitle.style = fields[3];
	subtitle.marginL = std::stoi(fields[5]);
	subtitle.marginR = std::stoi(fields[6]);
	subtitle.marginV = std::stoi(fields[7]);
	subtitle.text = dialogue_text;

	entries.push_back(subtitle);
}

void ASSA::removing_styling()
{
	static std::regex formatting_tags(R"(\{\\?[^}]+\}|\\[nNh]|\\i[0-9]|\\b[0-9]|\\u[0-9]|\\s[0-9]|\\[kKfo]?[0-9]+)", std::regex::icase);
	for (Subtitle_data& subtitle : entries)
	{
		subtitle.text = std::regex_replace(subtitle.text, formatting_tags, "");
	}
}

void ASSA::apply_default_style()
{
	styles.clear();
	styles.push_back("Style: Default,Arial,20,&H00FFFFFF,&H000000FF,&H00000000,&H00000000,"
					 "0,0,0,0,100,100,0,0,1,2,0,2,10,10,10,0");

	for (Subtitle_data& subtitle : entries)
	{
		subtitle.style = "Default";
		subtitle.text = "{\\i1}" + subtitle.text + "{\\i0}";
	}
}

void ASSA::shifting_time(int delta_ms, bool shift_start, bool shift_end)
{
	for (Subtitle_data& subtitle : entries)
	{
		if (shift_start)
		{
			subtitle.start_time = std::max(0ll, subtitle.start_time + delta_ms);
		}
		if (shift_end)
		{
			subtitle.end_time = std::max(subtitle.start_time + 1, subtitle.end_time + delta_ms);
		}
	}
}

void ASSA::reading_from_file(const std::string& file_path)
{
	std::ifstream file(file_path);
	if (!file.is_open())
	{
		throw std::runtime_error("Error opening file: " + file_path);
	}

	styles.clear();
	entries.clear();
	script_info.clear();

	std::string line;
	std::string section;

	while (std::getline(file, line))
	{
		if (line.empty())
		{
			continue;
		}

		if (line[0] == '[')
		{
			section = line;
			if (section == "[Script Info]")
			{
				script_info = line + "\n";
			}
			continue;
		}

		if (section == "[Script Info]")
		{
			script_info += line + "\n";
		}
		else if (section == "[V4+ Styles]" || section == "[Styles]")
		{
			styles.push_back(line);
		}
		else if (section == "[Events]" && line.find("Dialogue:") == 0)
		{
			parse_script_info(line);
		}
	}

	file.close();
}

void ASSA::writing_to_file(const std::string& file_path) const
{
	std::ofstream file(file_path);
	if (!file.is_open())
	{
		throw std::runtime_error("Error opening file: " + file_path);
	}

	file << "[Script Info]\n";
	file << script_info;

	file << "[V4+ Styles]\n";
	file << "Format: Layer, Start, End, Style, Name, MarginL, MarginR, MarginV, Text\n";

	for (const std::string& style : styles)
	{
		if (style.find("Style: ") == 0)
		{
			file << style << "\n";
		}
	}

	file << "[Events]\n";
	file << "Format: Layer, Start, End, Style, Name, MarginL, MarginR, MarginV, Text\n";

	for (const Subtitle_data& subtitle : entries)
	{
		file << "Dialogue: " << subtitle.layer << "," << formatting_time(subtitle.start_time) << ","
			 << formatting_time(subtitle.end_time) << "," << (subtitle.style.empty() ? "Default" : subtitle.style) << ",,"
			 << subtitle.marginL << "," << subtitle.marginR << "," << subtitle.marginV << "," << subtitle.text << "\n";
	}

	file.close();
}

bool ASSA::is_file_valid(const std::string& file_path) const
{
	std::ifstream file(file_path);
	if (!file.is_open())
	{
		return false;
	}

	bool has_script_info = false;
	bool has_styles_section = false;
	bool has_events_section = false;
	bool has_dialogue_lines = false;
	bool has_valid_event_format = false;

	std::string line;
	std::string current_section;

	Vector< std::string > required_fields;
	required_fields.push_back("Layer");
	required_fields.push_back("Start");
	required_fields.push_back("End");
	required_fields.push_back("Style");
	required_fields.push_back("Name");
	required_fields.push_back("MarginL");
	required_fields.push_back("MarginR");
	required_fields.push_back("MarginV");
	required_fields.push_back("Text");

	while (std::getline(file, line))
	{
		if (line.empty())
			continue;

		if (line[0] == '[')
		{
			current_section = line;
			if (current_section == "[Script Info]")
			{
				has_script_info = true;
			}
			else if (current_section == "[V4+ Styles]" || current_section == "[Styles]")
			{
				has_styles_section = true;
			}
			else if (current_section == "[Events]")
			{
				has_events_section = true;
			}
		}
		else if (current_section == "[Events]")
		{
			if (line.find("Format:") == 0)
			{
				std::string format_fields = line.substr(7);
				std::transform(format_fields.begin(), format_fields.end(), format_fields.begin(), ::tolower);

				bool all_fields_present = true;
				for (const std::string& field : required_fields)
				{
					std::string lowered = field;
					std::transform(lowered.begin(), lowered.end(), lowered.begin(), ::tolower);
					if (format_fields.find(lowered) == std::string::npos)
					{
						all_fields_present = false;
						break;
					}
				}

				if (!all_fields_present)
				{
					return false;
				}

				has_valid_event_format = true;
			}
			else if (line.find("Dialogue:") == 0)
			{
				has_dialogue_lines = true;
			}
		}

		if (has_script_info && has_styles_section && has_events_section && has_dialogue_lines && has_valid_event_format)
		{
			break;
		}
	}

	file.close();

	return has_script_info && has_styles_section && has_events_section && has_dialogue_lines && has_valid_event_format;
}
