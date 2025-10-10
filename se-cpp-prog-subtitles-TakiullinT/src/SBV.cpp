#include "../include/SBV.h"

long long SBV::parsing_time(const std::string& time_str)
{
	int hours, minutes, seconds, milliseconds;
	if (sscanf(time_str.c_str(), "%d:%d:%d.%d", &hours, &minutes, &seconds, &milliseconds) != 4)
	{
		throw std::invalid_argument("Invalid SBV time format");
	}

	if (hours < 0 || minutes < 0 || minutes >= 60 || seconds < 0 || seconds >= 60 || milliseconds < 0 || milliseconds >= 1000)
	{
		throw std::invalid_argument("Time values out of range");
	}

	return (static_cast< long long >(hours) * 3600 + minutes * 60 + seconds) * 1000 + milliseconds;
}

std::string SBV::formatting_time(long long time_ms)
{
	int hours = time_ms / 3600000;
	int minutes = (time_ms % 3600000) / 60000;
	int seconds = (time_ms % 60000) / 1000;
	int milliseconds = time_ms % 1000;

	std::ostringstream formatted_time;
	formatted_time
		<< hours << ":" << std::setw(2) << std::setfill('0') << minutes << ":" << std::setw(2) << std::setfill('0')
		<< seconds << "." << std::setw(3) << std::setfill('0') << milliseconds;

	return formatted_time.str();
}

void SBV::reading_from_file(const std::string& file_path)
{
	std::ifstream file(file_path);
	if (!file)
	{
		throw std::runtime_error("Cannot open file: " + file_path);
	}

	std::string current_line;
	Subtitle_data current_entry;
	int current_id = 1;
	bool reading_text = false;

	while (std::getline(file, current_line))
	{
		current_line.erase(std::remove(current_line.begin(), current_line.end(), '\r'), current_line.end());

		if (current_line.empty())
		{
			if (!current_entry.text.empty())
			{
				current_entry.id = current_id++;
				entries.push_back(current_entry);
				current_entry = Subtitle_data();
			}
			reading_text = false;
			continue;
		}

		if (!reading_text)
		{
			std::regex time_regex(R"((\d+:\d{2}:\d{2}\.\d{3}),(\d+:\d{2}:\d{2}\.\d{3}))");
			std::smatch match;
			if (std::regex_match(current_line, match, time_regex))
			{
				current_entry.start_time = parsing_time(match[1].str());
				current_entry.end_time = parsing_time(match[2].str());
				reading_text = true;
			}
			else
			{
				throw std::runtime_error("Invalid SBV time format: " + current_line);
			}
		}
		else
		{
			if (!current_entry.text.empty())
			{
				current_entry.text += "\n";
			}
			current_entry.text += current_line;
		}
	}

	if (!current_entry.text.empty())
	{
		current_entry.id = current_id++;
		entries.push_back(current_entry);
	}
}

void SBV::writing_to_file(const std::string& file_path) const
{
	std::ofstream file(file_path);
	if (!file)
	{
		throw std::runtime_error("Cannot open file: " + file_path);
	}

	for (const Subtitle_data& entry : entries)
	{
		file << formatting_time(entry.start_time) << "," << formatting_time(entry.end_time) << "\n";
		file << entry.text << "\n\n";
	}
}

bool SBV::is_file_valid(const std::string& file_path) const
{
	std::ifstream file(file_path);
	if (!file)
	{
		return false;
	}

	std::string line;
	bool has_time = false;
	bool has_text = false;

	while (std::getline(file, line))
	{
		line.erase(std::remove(line.begin(), line.end(), '\r'), line.end());

		if (line.empty())
		{
			continue;
		}

		if (!has_time)
		{
			std::regex time_regex(R"((\d+:\d{2}:\d{2}\.\d{3}),(\d+:\d{2}:\d{2}\.\d{3}))");
			if (std::regex_match(line, time_regex))
			{
				has_time = true;
			}
		}
		else if (!has_text)
		{
			has_text = true;
		}

		if (has_time && has_text)
		{
			break;
		}
	}

	return has_time && has_text;
}

void SBV::removing_styling()
{
	static std::regex tag_regex(R"(<[^>]*>)");
	for (Subtitle_data& entry : entries)
	{
		entry.text = std::regex_replace(entry.text, tag_regex, "");
	}
}

void SBV::apply_default_style()
{
	std::cerr << "Warning: SBV does not support styling! Use SRT/ASS instead.\n";

	static std::regex tag_regex("<[^>]*>");
	for (Subtitle_data& entry : entries)
	{
		entry.text = std::regex_replace(entry.text, tag_regex, "");
	}
}

void SBV::shifting_time(int delta_ms, bool shift_start, bool shift_end)
{
	for (Subtitle_data& entry : entries)
	{
		if (shift_start)
		{
			int new_start = entry.start_time + delta_ms;
			if (new_start < 0)
			{
				throw std::invalid_argument("Start time cannot be negative");
			}
			entry.start_time = new_start;
		}
		if (shift_end)
		{
			int new_end = entry.end_time + delta_ms;
			if (new_end < 0 || new_end <= entry.start_time)
			{
				throw std::invalid_argument("Invalid end time after shifting");
			}
			entry.end_time = new_end;
		}
	}
}
