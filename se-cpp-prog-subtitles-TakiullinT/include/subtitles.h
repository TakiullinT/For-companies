#pragma once
#include "../include/VECTOR.hpp"

#include <string>

struct Subtitle_data
{
	int id;
	std::string text;
	long long start_time;
	long long end_time;

	std::string style;
	int layer = 0;
	int marginL = 0;
	int marginR = 0;
	int marginV = 0;
};

class Subtitles
{
  public:
	virtual ~Subtitles() = default;

	virtual bool is_file_valid(const std::string& file_path) const = 0;
	virtual void reading_from_file(const std::string& file_path) = 0;
	virtual void writing_to_file(const std::string& file_path) const = 0;

	virtual void removing_styling() = 0;
	virtual void apply_default_style() = 0;

	virtual void set_entries(const Vector< Subtitle_data >& new_entries);
	virtual void shifting_time(int delta_ms, bool shift_start, bool shift_end) = 0;

	virtual Vector< Subtitle_data > finding_collisions() const;

	virtual const Vector< Subtitle_data >& getting_entries() const;

	virtual void add_entry(const Subtitle_data& entry);

  protected:
	Vector< Subtitle_data > entries;
};
