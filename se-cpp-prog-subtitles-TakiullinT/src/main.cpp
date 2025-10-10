#include "../include/ASSA.h"
#include "../include/LRC.h"
#include "../include/SAMI.h"
#include "../include/SBV.h"
#include "../include/SRT.h"
#include "../include/subtitles.h"

#include <iostream>
#include <stdexcept>

Subtitles* detect_subtitle_format(const std::string& filename)
{
	SRT srt;
	if (srt.is_file_valid(filename))
	{
		return new SRT();
	}

	SAMI sami;
	if (sami.is_file_valid(filename))
	{
		return new SAMI();
	}

	ASSA assa;
	if (assa.is_file_valid(filename))
	{
		return new ASSA();
	}

	SBV sbv;
	if (sbv.is_file_valid(filename))
	{
		return new SBV();
	}

	LRC lrc;
	if (lrc.is_file_valid(filename))
	{
		return new LRC();
	}

	throw std::runtime_error("Unsupported subtitle format");
}

Subtitles* create_subtitle_output(const std::string& filename)
{
	size_t dot_position = filename.find_last_of(".");
	std::string file_extension = filename.substr(dot_position + 1);

	for (char& c : file_extension)
	{
		if (c >= 'A' && c <= 'Z')
		{
			c += 32;
		}
	}

	if (file_extension == "srt")
	{
		return new SRT();
	}
	if (file_extension == "smi")
	{
		return new SAMI();
	}
	if (file_extension == "ass" || file_extension == "ssa")
	{
		return new ASSA();
	}
	if (file_extension == "sbv")
	{
		return new SBV();
	}
	if (file_extension == "lrc")
	{
		return new LRC();
	}

	throw std::runtime_error("Unsupported output subtitle format");
}

int main(int argc, char* argv[])
{
	if (argc != 3)
	{
		std::cerr << "Usage: " << argv[0] << " <input> <output>\n";
		return 1;
	}

	try
	{
		Subtitles* input_subtitle = detect_subtitle_format(argv[1]);
		input_subtitle->reading_from_file(argv[1]);
		input_subtitle->removing_styling();
		input_subtitle->apply_default_style();

		Subtitles* output_subtitle = create_subtitle_output(argv[2]);
		output_subtitle->set_entries(input_subtitle->getting_entries());
		output_subtitle->writing_to_file(argv[2]);

		delete input_subtitle;
		delete output_subtitle;
	} catch (const std::exception& e)
	{
		std::cerr << "Error: " << e.what() << "\n";
		return 2;
	}

	std::cout << "Success!\n";
	return 0;
}
