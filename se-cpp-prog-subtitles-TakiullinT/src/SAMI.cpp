#include "../include/SAMI.h"

long long SAMI::parsing_time(const std::string& time_str)
{
	try
	{
		return std::stoll(time_str);
	} catch (const std::exception& e)
	{
		throw std::runtime_error("Invalid time value: " + time_str);
	}
}

void SAMI::shifting_time(int delta_ms, bool shift_start, bool shift_end)
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

void SAMI::removing_styling()
{
	std::regex remove_tags(R"(<\/?[^>]+>|\{[^}]*\})");
	for (Subtitle_data& subtitle : entries)
	{
		subtitle.text = std::regex_replace(subtitle.text, remove_tags, "");
	}
}

void SAMI::apply_default_style()
{
	std::regex p_tag_regex(R"(<P[^>]*>)", std::regex::icase);
	std::string default_p_tag = "<P style=\"font-family:Arial; font-size:10pt; color:black; text-align:center\">";

	for (Subtitle_data& subtitle : entries)
	{
		subtitle.text = std::regex_replace(subtitle.text, p_tag_regex, default_p_tag);
	}
}

void SAMI::reading_from_file(const std::string& file_path)
{
	std::ifstream input_file(file_path);
	if (!input_file.is_open())
	{
		std::cerr << "Error opening file: " << file_path << std::endl;
		return;
	}

	entries.clear();

	std::stringstream file_buffer;
	file_buffer << input_file.rdbuf();
	std::string file_content = file_buffer.str();

	file_content = std::regex_replace(file_content, std::regex("\n"), "");
	file_content = std::regex_replace(file_content, std::regex("\r"), "");

	std::regex sync_block_regex(R"(<SYNC\s+Start\s*=\s*(\d+)(?:\s+End\s*=\s*(\d+))?>\s*<P[^>]*>(.*?)<\/P>)", std::regex::icase);
	std::smatch match_result;

	int subtitle_id = 1;
	long long last_end_time = 0;

	std::string::const_iterator search_start = file_content.cbegin();
	std::string::const_iterator search_end = file_content.cend();

	while (std::regex_search(search_start, search_end, match_result, sync_block_regex))
	{
		long long start_time = parsing_time(match_result[1].str());
		long long end_time = match_result[2].matched ? parsing_time(match_result[2].str()) : start_time + 1000;
		std::string subtitle_text = match_result[3].str();

		if (start_time < last_end_time)
		{
			start_time = last_end_time;
		}

		if (!subtitle_text.empty())
		{
			Subtitle_data subtitle;
			subtitle.id = subtitle_id++;
			subtitle.text = subtitle_text;
			subtitle.start_time = start_time;
			subtitle.end_time = end_time;
			entries.push_back(subtitle);

			last_end_time = end_time;
		}

		search_start = match_result.suffix().first;
	}

	input_file.close();
}

void SAMI::writing_to_file(const std::string& file_path) const
{
	std::ofstream output_file(file_path);
	if (!output_file.is_open())
	{
		throw std::runtime_error("Error opening file: " + file_path);
	}

	output_file << "<SAMI>\n";
	output_file << "<BODY>\n";

	for (const Subtitle_data& subtitle : entries)
	{
		output_file << "<SYNC Start=" << subtitle.start_time << " End=" << subtitle.end_time << ">\n";
		output_file << "<P>" << subtitle.text << "</P></SYNC>\n";
	}

	output_file << "</BODY>\n";
	output_file << "</SAMI>\n";

	output_file.close();
}

bool SAMI::is_file_valid(const std::string& file_path) const
{
	std::ifstream input_file(file_path);
	if (!input_file.is_open())
	{
		return false;
	}

	std::string content((std::istreambuf_iterator< char >(input_file)), std::istreambuf_iterator< char >());
	input_file.close();

	for (size_t i = 0; i < content.size(); i++)
	{
		content[i] = std::toupper(content[i]);
	}

	bool has_open_sami = content.find("<SAMI>") != std::string::npos;
	bool has_close_sami = content.find("</SAMI>") != std::string::npos;
	bool has_open_body = content.find("<BODY>") != std::string::npos;
	bool has_close_body = content.find("</BODY>") != std::string::npos;

	if (!has_open_sami || !has_close_sami || !has_open_body || !has_close_body)
	{
		return false;
	}

	size_t current_position = 0;
	while (true)
	{
		size_t sync_tag_start = content.find("<SYNC", current_position);
		if (sync_tag_start == std::string::npos)
		{
			break;
		}

		size_t sync_tag_close = content.find('>', sync_tag_start);
		if (sync_tag_close == std::string::npos)
		{
			return false;
		}

		size_t sync_end_tag = content.find("</SYNC>", sync_tag_close);
		if (sync_end_tag == std::string::npos)
		{
			return false;
		}

		std::string sync_inner_content = content.substr(sync_tag_close + 1, sync_end_tag - sync_tag_close - 1);

		std::string stripped_text;
		for (size_t i = 0; i < sync_inner_content.size(); ++i)
		{
			char ch = sync_inner_content[i];
			if (ch != '\n' && ch != '\r' && ch != ' ' && ch != '\t')
			{
				stripped_text += ch;
			}
		}

		bool has_valid_p_tag = stripped_text.find("<P>") == 0 && stripped_text.rfind("</P>") == stripped_text.length() - 4;
		if (!has_valid_p_tag)
		{
			return false;
		}

		current_position = sync_end_tag + 7;
	}

	return true;
}
