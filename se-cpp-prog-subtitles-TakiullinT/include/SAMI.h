#pragma once
#include "subtitles.h"

#include <fstream>
#include <iostream>
#include <regex>
#include <sstream>
#include <string>

class SAMI : public Subtitles
{
  public:
	SAMI() = default;
	~SAMI() override = default;

	bool is_file_valid(const std::string& file_path) const override;
	void reading_from_file(const std::string& file_path) override;
	void writing_to_file(const std::string& file_path) const override;

	void removing_styling() override;
	void apply_default_style() override;

	void shifting_time(int delta_ms, bool shift_start, bool shift_end) override;

  private:
	static long long parsing_time(const std::string& time_str);
};
