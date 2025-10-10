#include "../include/SRT.h"

#include <iomanip>
#include <regex>
#include <sstream>
#include <stdexcept>

long long SRT::parsing_time(const std::string& time_str)
{
	int hours, minutes, seconds, milliseconds;
	std::sscanf(time_str.c_str(), "%d:%d:%d,%d", &hours, &minutes, &seconds, &milliseconds);
	if (hours < 0 || minutes < 0 || seconds < 0 || milliseconds < 0 || hours > 23 || minutes > 59 || seconds > 59 || milliseconds > 999)
	{
		throw std::invalid_argument("Invalid time format");
	}
	return (((hours * 3600 + minutes * 60 + seconds) * 1000) + milliseconds);
}

std::string SRT::formatting_time(long long time_ms)
{
	if (time_ms < 0)
	{
		throw std::invalid_argument("Time cannot be negative");
	}

	int hours = time_ms / 3600000;
	int minutes = (time_ms % 3600000) / 60000;
	int seconds = (time_ms % 60000) / 1000;
	int milliseconds = time_ms % 1000;

	std::ostringstream formatted_time;
	formatted_time
		<< std::setw(2) << std::setfill('0') << hours << ":" << std::setw(2) << std::setfill('0') << minutes << ":"
		<< std::setw(2) << std::setfill('0') << seconds << "," << std::setw(3) << std::setfill('0') << milliseconds;

	return formatted_time.str();
}

void SRT::shifting_time(int delta_ms, bool shift_start, bool shift_end)
{
	for (Subtitle_data& subtitle : entries)
	{
		if (shift_start)
		{
			int updated_start = subtitle.start_time + delta_ms;
			if (updated_start < 0)
			{
				throw std::invalid_argument("Start time cannot be negative");
			}
			subtitle.start_time = updated_start;
		}

		if (shift_end)
		{
			int updated_end = subtitle.end_time + delta_ms;
			if (updated_end < 0 || updated_end <= subtitle.start_time)
			{
				throw std::invalid_argument("Invalid end time after shifting");
			}
			subtitle.end_time = updated_end;
		}
	}
}

void SRT::removing_styling()
{
	static std::regex styling_tags(R"((<\/?[biu]>\s*)|(\{\\?[a-zA-Z]+\d*[^}]*\}))");
	for (Subtitle_data& subtitle : entries)
	{
		subtitle.text = std::regex_replace(subtitle.text, styling_tags, "");
	}
}

void SRT::apply_default_style()
{
	for (Subtitle_data& subtitle : entries)
	{
		if (subtitle.text.find("<i>") != std::string::npos || subtitle.text.find("<b>") != std::string::npos ||
			subtitle.text.find("<u>") != std::string::npos)
		{
			continue;
		}
		subtitle.text = "<i>" + subtitle.text + "</i>";
	}
}

void SRT::reading_from_file(const std::string& file_path)
{
	std::ifstream input_file(file_path);
	if (!input_file)
	{
		throw std::runtime_error("Error opening file: " + file_path);
	}

	std::string current_line;
	Subtitle_data current_subtitle;
	int reading_state = 0;

	while (getline(input_file, current_line))
	{
		current_line.erase(std::remove(current_line.begin(), current_line.end(), '\r'), current_line.end());

		if (current_line.empty())
		{
			if (!current_subtitle.text.empty())
			{
				entries.push_back(current_subtitle);
				current_subtitle = Subtitle_data();
			}
			reading_state = 0;
			continue;
		}

		switch (reading_state)
		{
		case 0:
			current_subtitle.id = std::stoi(current_line);
			reading_state = 1;
			break;
		case 1:
		{
			std::regex timestamp_pattern(R"((\d{2}:\d{2}:\d{2},\d{3})\s*-->\s*(\d{2}:\d{2}:\d{2},\d{3}))");
			std::smatch match;
			if (std::regex_match(current_line, match, timestamp_pattern))
			{
				current_subtitle.start_time = parsing_time(match[1].str());
				current_subtitle.end_time = parsing_time(match[2].str());
				reading_state = 2;
			}
			else
			{
				throw std::runtime_error("Invalid time format: " + current_line);
			}
			break;
		}
		case 2:
			if (!current_subtitle.text.empty())
				current_subtitle.text += "\n";
			current_subtitle.text += current_line;
			break;
		}
	}
	if (!current_subtitle.text.empty())
	{
		entries.push_back(current_subtitle);
	}
}

void SRT::writing_to_file(const std::string& file_path) const
{
	std::ofstream output_file(file_path);
	if (!output_file)
	{
		throw std::runtime_error("Error opening file: " + file_path);
	}

	for (const Subtitle_data& subtitle : entries)
	{
		output_file << subtitle.id << "\n";
		output_file << formatting_time(subtitle.start_time) << " --> " << formatting_time(subtitle.end_time) << "\n";
		output_file << subtitle.text << "\n\n";
	}
}

bool SRT::is_file_valid(const std::string& file_path) const
{
	std::ifstream input_file(file_path);
	if (!input_file)
	{
		return false;
	}

	std::string line;
	int parsing_stage = 0;
	int valid_block_count = 0;

	while (getline(input_file, line))
	{
		line.erase(std::remove(line.begin(), line.end(), '\r'), line.end());

		if (line.empty())
		{
			if (parsing_stage == 2)
			{
				valid_block_count++;
				parsing_stage = 0;
			}
			continue;
		}

		switch (parsing_stage)
		{
		case 0:
		{
			bool has_non_digit = false;
			for (char ch : line)
			{
				if (!std::isdigit(static_cast< unsigned char >(ch)))
				{
					has_non_digit = true;
					break;
				}
			}
			if (has_non_digit)
			{
				return false;
			}
			parsing_stage = 1;
			break;
		}
		case 1:
			if (!std::regex_match(line, std::regex(R"((\d{2}:\d{2}:\d{2},\d{3})\s*-->\s*(\d{2}:\d{2}:\d{2},\d{3}))")))
			{
				return false;
			}
			parsing_stage = 2;
			break;
		case 2:
			break;
		}
	}

	return valid_block_count > 0;
}
