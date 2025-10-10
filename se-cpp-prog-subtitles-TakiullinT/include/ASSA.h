#pragma once
#include "VECTOR.hpp"
#include "subtitles.h"

#include <fstream>
#include <iostream>
#include <regex>
#include <sstream>
#include <string>

class ASSA : public Subtitles
{
  private:
	std::string script_info;
	Vector< std::string > styles;

	static long long parsing_time(const std::string& time_str);
	static std::string formatting_time(long long time);
	void parse_script_info(const std::string& line);

  public:
	ASSA() = default;
	~ASSA() override = default;

	bool is_file_valid(const std::string& file_path) const override;
	void reading_from_file(const std::string& file_path) override;
	void writing_to_file(const std::string& file_path) const override;

	void removing_styling() override;
	void apply_default_style() override;

	void shifting_time(int delta_ms, bool shift_start, bool shift_end) override;

	const Vector< std::string >& get_styles() const;
};
