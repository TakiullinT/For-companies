#include "LRC.h"

long long LRC::parsing_time(const std::string& time_str)
{
	int minutes, seconds, hundredths = 0;
	if (sscanf(time_str.c_str(), "%d:%d.%d", &minutes, &seconds, &hundredths) == 3)
	{
		return minutes * 60000LL + seconds * 1000LL + hundredths * 10LL;
	}
	if (sscanf(time_str.c_str(), "%d:%d", &minutes, &seconds) == 2)
	{
		return minutes * 60000LL + seconds * 1000LL;
	}
	throw std::invalid_argument("Invalid LRC time format: " + time_str);
}

std::string LRC::formatting_time(long long ms) const
{
	if (ms < 0)
	{
		throw std::invalid_argument("Negative time");
	}
	int total_seconds = ms / 1000;
	int minutes = total_seconds / 60;
	int seconds = total_seconds % 60;
	int centiseconds = (ms % 1000) / 10;

	std::ostringstream oss;
	oss << std::setw(2) << std::setfill('0') << minutes << ":" << std::setw(2) << std::setfill('0') << seconds << "."
		<< std::setw(2) << std::setfill('0') << centiseconds;

	return oss.str();
}

bool LRC::is_file_valid(const std::string& file_path) const
{
	std::ifstream input_file(file_path);
	if (!input_file)
	{
		return false;
	}

	std::string current_line;
	std::regex timestamp_regex(R"(\[(\d{2}):(\d{2})\.(\d{2,3})\])");
	bool found_valid_timestamp = false;

	while (std::getline(input_file, current_line))
	{
		size_t position = 0;
		while (true)
		{
			size_t open_bracket_pos = current_line.find('[', position);
			size_t close_bracket_pos = current_line.find(']', open_bracket_pos);
			if (open_bracket_pos == std::string::npos || close_bracket_pos == std::string::npos)
			{
				break;
			}

			std::string timestamp_tag = current_line.substr(open_bracket_pos, close_bracket_pos - open_bracket_pos + 1);

			if (timestamp_tag.size() >= 6 && std::isdigit(timestamp_tag[1]) && std::isdigit(timestamp_tag[2]) && timestamp_tag[3] == ':')
			{
				found_valid_timestamp = true;

				if (!std::regex_match(timestamp_tag, timestamp_regex))
				{
					return false;
				}
			}

			position = close_bracket_pos + 1;
		}
	}

	return found_valid_timestamp;
}

void LRC::reading_from_file(const std::string& file_path)
{
	std::ifstream input_file(file_path);
	if (!input_file)
	{
		throw std::runtime_error("Cannot open LRC: " + file_path);
	}

	entries.clear();
	std::string line;
	std::regex timestamp_tag_regex(R"(\[(\d{2}:\d{2}\.\d{2,3})\])");
	int subtitle_id = 1;

	while (std::getline(input_file, line))
	{
		std::smatch match;
		Vector< long long > timestamps;

		std::string::const_iterator search_begin = line.cbegin(), search_end = line.cend();

		while (std::regex_search(search_begin, search_end, match, timestamp_tag_regex))
		{
			timestamps.push_back(parsing_time(match[1].str()));
			search_begin = match.suffix().first;
		}

		if (timestamps.get_size() > 0)
		{
			std::string subtitle_text = line.substr(line.find(']') + 1);
			for (long long timestamp : timestamps)
			{
				Subtitle_data subtitle;
				subtitle.id = subtitle_id++;
				subtitle.start_time = timestamp;
				subtitle.end_time = timestamp + 2000;
				subtitle.text = subtitle_text;
				entries.push_back(subtitle);
			}
		}
	}
}

void LRC::writing_to_file(const std::string& file_path) const
{
	std::ofstream output_file(file_path);
	if (!output_file)
	{
		throw std::runtime_error("Cannot open LRC for write: " + file_path);
	}

	for (const Subtitle_data& subtitle : entries)
	{
		output_file << "[" << formatting_time(subtitle.start_time) << "]" << subtitle.text << "\n";
	}
}

void LRC::removing_styling()
{
	static std::regex html_tag_regex(R"(<[^>]+>)");

	for (Subtitle_data& subtitle : entries)
	{
		subtitle.text = std::regex_replace(subtitle.text, html_tag_regex, "");
	}
}

void LRC::apply_default_style()
{
	for (Subtitle_data& subtitle : entries)
	{
		std::string& raw_text = subtitle.text;
		std::string cleaned_text;
		bool inside_tag = false;

		for (char ch : raw_text)
		{
			if (ch == '<')
			{
				inside_tag = true;
			}
			else if (ch == '>')
			{
				inside_tag = false;
			}
			else if (!inside_tag)
			{
				cleaned_text += ch;
			}
		}

		subtitle.text = cleaned_text;
	}
}

void LRC::shifting_time(int delta_ms, bool shift_start, bool shift_end)
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
