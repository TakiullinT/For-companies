#include "../include/subtitles.h"

#include "../include/VECTOR.hpp"

void Subtitles::set_entries(const Vector< Subtitle_data >& new_entries)
{
	this->entries = new_entries;
}

const Vector< Subtitle_data >& Subtitles::getting_entries() const
{
	return entries;
}

Vector< Subtitle_data > Subtitles::finding_collisions() const
{
	Vector< Subtitle_data > collisions;
	Vector< bool > visited(entries.get_size(), false);

	for (size_t i = 0; i < entries.get_size(); ++i)
	{
		if (visited[i])
		{
			continue;
		}

		for (size_t j = i + 1; j < entries.get_size(); ++j)
		{
			if ((entries[i].start_time < entries[j].end_time) && (entries[j].start_time < entries[i].end_time))
			{
				if (!visited[i])
				{
					collisions.push_back(entries[i]);
					visited[i] = true;
				}
				if (!visited[j])
				{
					collisions.push_back(entries[j]);
					visited[j] = true;
				}
			}
		}
	}
	return collisions;
}

void Subtitles::add_entry(const Subtitle_data& entry)
{
	entries.push_back(entry);
}
