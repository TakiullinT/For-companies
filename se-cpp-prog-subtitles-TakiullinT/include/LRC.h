
#pragma once

#include "VECTOR.hpp"
#include "subtitles.h"

#include <fstream>
#include <iomanip>
#include <regex>
#include <sstream>
#include <stdexcept>
#include <string>

class LRC : public Subtitles
{
  public:
	LRC() = default;
	~LRC() override = default;

	bool is_file_valid(const std::string& file_path) const override;
	void reading_from_file(const std::string& file_path) override;
	void writing_to_file(const std::string& file_path) const override;

	void removing_styling() override;
	void apply_default_style() override;

	void shifting_time(int delta_ms, bool shift_start, bool shift_end) override;

  private:
	long long parsing_time(const std::string& time_str);
	std::string formatting_time(long long time_ms) const;
};
