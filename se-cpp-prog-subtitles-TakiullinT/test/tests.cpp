#include "../include/ASSA.h"
#include "../include/LRC.h"
#include "../include/SAMI.h"
#include "../include/SBV.h"
#include "../include/SRT.h"

#include <gtest/gtest.h>

TEST(SRTTest, ValidFile)
{
	std::ofstream out("test.srt");
	out << "1\n00:00:01,000 --> 00:00:04,000\nHello, world!\n\n";
	out << "2\n00:00:03,500 --> 00:00:05,000\nThis is a subtitle test.\n\n";
	out << "3\n00:00:06,000 --> 00:00:07,000\nNo collision here.\n\n";
	out.close();

	SRT srt;
	EXPECT_TRUE(srt.is_file_valid("test.srt"));

	std::remove("test.srt");
}

TEST(SRTTest, InvalidFile)
{
	SRT srt;
	EXPECT_FALSE(srt.is_file_valid("invalid.srt"));
}

TEST(SRTTest, ReadWrite)
{
	std::ofstream out("test.srt");
	out << "1\n00:00:01,000 --> 00:00:04,000\nHello, world!\n\n";
	out << "2\n00:00:03,500 --> 00:00:05,000\nThis is a subtitle test.\n\n";
	out << "3\n00:00:06,000 --> 00:00:07,000\nNo collision here.\n\n";
	out.close();

	SRT srt;
	srt.reading_from_file("test.srt");
	srt.writing_to_file("output.srt");
	EXPECT_TRUE(srt.is_file_valid("output.srt"));

	std::remove("test.srt");
	std::remove("output.srt");
}

TEST(SRTTest, ShiftingTime)
{
	std::ofstream out("test.srt");
	out << "1\n00:00:01,000 --> 00:00:04,000\nHello, world!\n\n";
	out << "2\n00:00:03,500 --> 00:00:05,000\nThis is a subtitle test.\n\n";
	out << "3\n00:00:06,000 --> 00:00:07,000\nNo collision here.\n\n";
	out.close();

	SRT srt;
	srt.reading_from_file("test.srt");
	srt.shifting_time(1000, true, false);
	srt.writing_to_file("shifted.srt");
	EXPECT_TRUE(srt.is_file_valid("shifted.srt"));

	std::remove("test.srt");
	std::remove("shifted.srt");
}

TEST(SRTTest, LargeTimeValues)
{
	SRT srt;
	Subtitle_data entry{ 1, "Test", 1000000000, 1000001000 };
	srt.add_entry(entry);
	EXPECT_NO_THROW(srt.writing_to_file("large_time.srt"));

	std::remove("large_time.srt");
}

TEST(SRTTest, FindingCollisions)
{
	std::ofstream out("test.srt");
	out << "1\n00:00:01,000 --> 00:00:04,000\nHello, world!\n\n";
	out << "2\n00:00:03,500 --> 00:00:05,000\nThis is a subtitle test.\n\n";
	out << "3\n00:00:06,000 --> 00:00:07,000\nNo collision here.\n\n";
	out.close();

	SRT srt;
	srt.reading_from_file("test.srt");
	Vector< Subtitle_data > collisions = srt.finding_collisions();
	EXPECT_GT(collisions.get_size(), 0);

	std::remove("test.srt");
}

TEST(SRTTest, RemovingStyling)
{
	std::ofstream out("test.srt");
	out << "1\n00:00:01,000 --> 00:00:04,000\nHello, world!\n\n";
	out << "2\n00:00:03,500 --> 00:00:05,000\nThis is a subtitle test.\n\n";
	out << "3\n00:00:06,000 --> 00:00:07,000\nNo collision here.\n\n";
	out.close();

	SRT srt;
	srt.reading_from_file("test.srt");
	srt.removing_styling();
	srt.writing_to_file("removed_styles.srt");
	EXPECT_TRUE(srt.is_file_valid("removed_styles.srt"));

	std::remove("test.srt");
	std::remove("removed_styles.srt");
}

TEST(SRTTest, ApplyDefaultStyle)
{
	std::ofstream out("test.srt");
	out << "1\n00:00:01,000 --> 00:00:04,000\nHello, world!\n\n";
	out << "2\n00:00:03,500 --> 00:00:05,000\nThis is a subtitle test.\n\n";
	out << "3\n00:00:06,000 --> 00:00:07,000\nNo collision here.\n\n";
	out.close();

	SRT srt;
	srt.reading_from_file("test.srt");
	srt.apply_default_style();
	srt.writing_to_file("default_style.srt");
	EXPECT_TRUE(srt.is_file_valid("default_style.srt"));

	std::remove("test.srt");
	std::remove("default_style.srt");
}

TEST(SRTTest, AddEntry)
{
	std::ofstream out("test.srt");
	out << "1\n00:00:01,000 --> 00:00:04,000\nHello, world!\n\n";
	out << "2\n00:00:03,500 --> 00:00:05,000\nThis is a subtitle test.\n\n";
	out << "3\n00:00:06,000 --> 00:00:07,000\nNo collision here.\n\n";
	out.close();

	SRT srt;
	srt.reading_from_file("test.srt");
	Subtitle_data new_entry = { 1, "New Entry", 0, 1000 };
	srt.add_entry(new_entry);
	srt.writing_to_file("added_entry.srt");
	EXPECT_TRUE(srt.is_file_valid("added_entry.srt"));

	std::remove("test.srt");
	std::remove("added_entry.srt");
}

TEST(SRTTest, GetEntries)
{
	std::ofstream out("test.srt");
	out << "1\n00:00:01,000 --> 00:00:04,000\nHello, world!\n\n";
	out << "2\n00:00:03,500 --> 00:00:05,000\nThis is a subtitle test.\n\n";
	out << "3\n00:00:06,000 --> 00:00:07,000\nNo collision here.\n\n";
	out.close();

	SRT srt;
	srt.reading_from_file("test.srt");
	const Vector< Subtitle_data >& entries = srt.getting_entries();
	EXPECT_GT(entries.get_size(), 0);

	std::remove("test.srt");
}

TEST(SRTTest, reading_from_file)
{
	std::ofstream out("test.srt");
	out << "1\n00:00:01,000 --> 00:00:04,000\nHello, world!\n\n";
	out << "2\n00:00:03,500 --> 00:00:05,000\nThis is a subtitle test.\n\n";
	out << "3\n00:00:06,000 --> 00:00:07,000\nNo collision here.\n\n";
	out.close();

	SRT srt;
	srt.reading_from_file("test.srt");
	EXPECT_TRUE(srt.is_file_valid("test.srt"));

	std::remove("test.srt");
}

TEST(SRTTest, writing_to_file)
{
	std::ofstream out("test.srt");
	out << "1\n00:00:01,000 --> 00:00:04,000\nHello, world!\n\n";
	out << "2\n00:00:03,500 --> 00:00:05,000\nThis is a subtitle test.\n\n";
	out << "3\n00:00:06,000 --> 00:00:07,000\nNo collision here.\n\n";
	out.close();

	SRT srt;
	srt.reading_from_file("test.srt");
	srt.writing_to_file("output.srt");
	EXPECT_TRUE(srt.is_file_valid("output.srt"));

	std::remove("test.srt");
	std::remove("output.srt");
}

TEST(SAMITest, ValidFile)
{
	std::ofstream out("test.sami");
	out << "<SAMI>\n";
	out << "<BODY>\n";
	out << "<SYNC Start=1000 End=4000>\n";
	out << "<P>Hello, world!</P></SYNC>\n";
	out << "</BODY>\n";
	out << "</SAMI>\n";
	out.close();

	SAMI sami;
	EXPECT_TRUE(sami.is_file_valid("test.sami"));

	std::remove("test.sami");
}

TEST(SAMITest, InvalidFile)
{
	SAMI sami;
	EXPECT_FALSE(sami.is_file_valid("invalid.sami"));
}

TEST(SAMITest, ReadWrite)
{
	std::ofstream out("test.sami");
	out << "<SAMI>\n";
	out << "<BODY>\n";
	out << "<SYNC Start=1000 End=4000>\n";
	out << "<P>Hello, world!</P>\n";
	out << "</BODY>\n";
	out << "</SAMI>\n";
	out.close();

	SAMI sami;
	sami.reading_from_file("test.sami");
	sami.writing_to_file("output.sami");
	EXPECT_TRUE(sami.is_file_valid("output.sami"));

	std::remove("test.sami");
	std::remove("output.sami");
}

TEST(SAMITest, ShiftingTime)
{
	std::ofstream out("test.sami");
	out << "<SAMI>\n";
	out << "<BODY>\n";
	out << "<SYNC Start=1000 End=4000>\n";
	out << "<P>Hello, world!</P>\n";
	out << "</BODY>\n";
	out << "</SAMI>\n";
	out.close();

	SAMI sami;
	sami.reading_from_file("test.sami");
	sami.shifting_time(1000, true, false);
	sami.writing_to_file("shifted.sami");
	EXPECT_TRUE(sami.is_file_valid("shifted.sami"));

	std::remove("test.sami");
	std::remove("shifted.sami");
}

TEST(SAMITest, FindingCollisions)
{
	std::ofstream out("test.sami");
	out << "<SAMI>\n";
	out << "<BODY>\n";
	out << "<SYNC Start=1000 End=4000>\n";
	out << "<P>Hello, world!</P>\n";
	out << "</BODY>\n";
	out << "</SAMI>\n";
	out.close();

	SAMI sami;
	sami.reading_from_file("test.sami");
	Vector< Subtitle_data > collisions = sami.finding_collisions();
	EXPECT_EQ(collisions.get_size(), 0);

	std::remove("test.sami");
}
TEST(SAMITest, AddEntry)
{
	std::ofstream out("test.sami");
	out << "<SAMI>\n";
	out << "<BODY>\n";
	out << "<SYNC Start=1000 End=4000>\n";
	out << "<P>Hello, world!</P>\n";
	out << "</BODY>\n";
	out << "</SAMI>\n";
	out.close();

	SAMI sami;
	sami.reading_from_file("test.sami");
	Subtitle_data new_entry = { 1, "New Entry", 0, 1000 };
	sami.add_entry(new_entry);
	sami.writing_to_file("added_entry.sami");
	EXPECT_TRUE(sami.is_file_valid("added_entry.sami"));

	std::remove("test.sami");
	std::remove("added_entry.sami");
}

TEST(SAMITest, GetEntries)
{
	std::ofstream out("test.sami");
	out << "<SAMI>\n";
	out << "<BODY>\n";
	out << "<SYNC Start=1000 End=4000>\n";
	out << "<P>Hello, world!</P>\n";
	out << "</BODY>\n";
	out << "</SAMI>\n";
	out.close();

	SAMI sami;
	sami.reading_from_file("test.sami");
	const Vector< Subtitle_data >& entries = sami.getting_entries();
	EXPECT_GT(entries.get_size(), 0);

	std::remove("test.sami");
}
TEST(SAMITest, ApplyDefaultStyle)
{
	std::ofstream out("test.sami");
	out << "<SAMI>\n";
	out << "<BODY>\n";
	out << "<SYNC Start=1000 End=4000>\n";
	out << "<P Style=\"italic\">Hello, world!</P>\n";
	out << "</BODY>\n";
	out << "</SAMI>\n";
	out.close();

	SAMI sami;
	sami.reading_from_file("test.sami");
	sami.apply_default_style();
	sami.writing_to_file("default_style.sami");
	EXPECT_TRUE(sami.is_file_valid("default_style.sami"));

	std::remove("test.sami");
	std::remove("default_style.sami");
}
TEST(SAMITest, RemovingStyling)
{
	std::ofstream out("test.sami");
	out << "<SAMI>\n";
	out << "<BODY>\n";
	out << "<SYNC Start=1000 End=4000>\n";
	out << "<P Style=\"bold\">Hello, world!</P>\n";
	out << "</BODY>\n";
	out << "</SAMI>\n";
	out.close();

	SAMI sami;
	sami.reading_from_file("test.sami");
	sami.removing_styling();
	sami.writing_to_file("removed_styles.sami");
	EXPECT_TRUE(sami.is_file_valid("removed_styles.sami"));

	std::remove("test.sami");
	std::remove("removed_styles.sami");
}

TEST(SAMITest, ReadingFromFile)
{
	std::ofstream out("test.sami");
	out << "<SAMI>\n";
	out << "<BODY>\n";
	out << "<SYNC Start=1000 End=4000>\n";
	out << "<P>Hello, world!</P></SYNC>\n";
	out << "</BODY>\n";
	out << "</SAMI>\n";
	out.close();

	SAMI sami;
	sami.reading_from_file("test.sami");
	EXPECT_TRUE(sami.is_file_valid("test.sami"));

	std::remove("test.sami");
}

TEST(SAMITest, WritingToFile)
{
	std::ofstream out("test.sami");
	out << "<SAMI>\n";
	out << "<BODY>\n";
	out << "<SYNC Start=1000 End=4000>\n";
	out << "<P>Hello, world!</P>\n";
	out << "</BODY>\n";
	out << "</SAMI>\n";
	out.close();

	SAMI sami;
	sami.reading_from_file("test.sami");
	sami.writing_to_file("output.sami");
	EXPECT_TRUE(sami.is_file_valid("output.sami"));

	std::remove("test.sami");
	std::remove("output.sami");
}

TEST(ASSATest, ValidFile)
{
	const std::string test_file = "valid_no_extra.ass";
	{
		std::ofstream out(test_file);
		out << "[Script Info]\n"
			<< "Title: Test Subtitle\n"
			<< "ScriptType: v4.00+\n\n"
			<< "[V4+ Styles]\n"
			<< "Format: Layer, Start, End, Style, Name, MarginL, MarginR, MarginV, Text\n"
			<< "Style: Default,Arial,20\n\n"
			<< "[Events]\n"
			<< "Format: Layer, Start, End, Style, Name, MarginL, MarginR, MarginV, Text\n"
			<< "Dialogue: 0,0:00:01.00,0:00:03.00,Default,,0,0,0,First subtitle\n"
			<< "Dialogue: 0,0:00:01.00,0:00:03.00,Default,,0,0,0,Second subtitle\n"
			<< "Dialogue: 0,0:00:01.00,0:00:03.00,Default,,0,0,0,No collision\n";
	}

	ASSA assa;
	EXPECT_TRUE(assa.is_file_valid(test_file));

	assa.reading_from_file(test_file);
	const Vector< Subtitle_data >& entries = assa.getting_entries();
	EXPECT_EQ(entries.get_size(), 3);
	EXPECT_EQ(entries[0].text, "First subtitle");
	EXPECT_EQ(entries[1].text, "Second subtitle");
	EXPECT_EQ(entries[2].text, "No collision");

	std::remove(test_file.c_str());
}

TEST(ASSATest, EmptyFile)
{
	const std::string filename = "empty_test.ass";
	{
		std::ofstream file(filename);
	}

	ASSA assa;
	EXPECT_FALSE(assa.is_file_valid(filename));

	std::remove(filename.c_str());
}

TEST(ASSATest, DefaultStyle)
{
	const std::string file = "test.ass";
	{
		std::ofstream out(file);
		out << "[Script Info]\n"
			<< "Title: Default Test\n"
			<< "ScriptType: v4.00+\n\n"
			<< "[Events]\n"
			<< "Format: Layer, Start, End, Style, Text\n"
			<< "Dialogue: 0,0:00:01.00,0:00:03.00,Default,,0,0,0,First line\n"
			<< "Dialogue: 0,0:00:03.00,0:00:05.00,Default,,0,0,0,{\\b1}Bold text\n";
	}

	ASSA assa;
	assa.reading_from_file(file);

	assa.apply_default_style();
	const Vector< Subtitle_data >& entries = assa.getting_entries();
	for (size_t i = 0; i < entries.get_size(); ++i)
	{
		EXPECT_EQ(entries[i].style, "Default");
	}

	assa.writing_to_file("output.ass");
	std::ifstream in("output.ass");
	std::string line;
	bool has_default_style = false;
	while (std::getline(in, line))
	{
		if (line.find("Style: Default") != std::string::npos)
		{
			has_default_style = true;
			break;
		}
	}
	EXPECT_TRUE(has_default_style);

	std::remove(file.c_str());
	std::remove("output.ass");
}

TEST(ASSATest, TimeShifting)
{
	const std::string file = "test.ass";
	{
		std::ofstream out(file);
		out << "[Script Info]\n"
			<< "Title: Time Test\n"
			<< "ScriptType: v4.00+\n\n"
			<< "[Events]\n"
			<< "Format: Layer, Start, End, Style, Text\n"
			<< "Dialogue: 0,0:00:01.00,0:00:03.00,Default,,0,0,0,First subtitle\n";
	}

	ASSA assa;
	assa.reading_from_file(file);

	assa.shifting_time(500, true, true);

	const Vector< Subtitle_data >& entries = assa.getting_entries();
	EXPECT_EQ(entries[0].start_time, 1500);
	EXPECT_EQ(entries[0].end_time, 3500);

	assa.shifting_time(-2000, true, false);
	EXPECT_EQ(entries[0].start_time, 0);
	EXPECT_EQ(entries[0].end_time, 3500);

	std::remove(file.c_str());
}

TEST(ASSATest, Collisions)
{
	const std::string filename = "collision_test.ass";
	{
		std::ofstream out(filename);
		out << "[Script Info]\n...\n"
			<< "Title: Collision Test\n\n"
			<< "[V4+ Styles]\n"
			<< "Format: Name, Fontname, Fontsize\n"
			<< "Style: Default,Arial,20\n\n"
			<< "[Events]\n"
			<< "Dialogue: 0,0:00:01.00,0:00:03.00,Default,,0,0,0,First\n"
			<< "Dialogue: 0,0:00:02.00,0:00:04.00,Default,,0,0,0,Second\n"
			<< "Dialogue: 0,0:00:05.00,0:00:06.00,Default,,0,0,0,Third\n";
	}

	ASSA assa;
	assa.reading_from_file(filename);

	Vector< Subtitle_data > collisions = assa.finding_collisions();
	EXPECT_EQ(collisions.get_size(), 2);

	Subtitle_data new_entry{ 0, "New", 2000, 3000 };
	assa.add_entry(new_entry);

	collisions = assa.finding_collisions();
	EXPECT_EQ(collisions.get_size(), 3);

	std::remove(filename.c_str());
}

TEST(ASSATest, RemoveStyling)
{
	const std::string file = "test.ass";
	{
		std::ofstream out(file);
		out << "[Script Info]\n"
			<< "Title: Styling Test\n"
			<< "ScriptType: v4.00+\n\n"
			<< "[Events]\n"
			<< "Format: Layer, Start, End, Style, Text\n"
			<< "Dialogue: 0,0:00:01.00,0:00:03.00,Default,,0,0,0,First line\n"
			<< "Dialogue: 0,0:00:03.00,0:00:05.00,Default,,0,0,0,{\\b1}Bold text\n";
	}

	ASSA assa;
	assa.reading_from_file(file);

	const Vector< Subtitle_data >& entries = assa.getting_entries();
	EXPECT_NE(entries[1].text.find("\\b1"), std::string::npos);

	assa.removing_styling();
	EXPECT_EQ(entries[1].text, "Bold text");

	std::remove(file.c_str());
}

TEST(ASSATest, GetEntries)
{
	const std::string file = "test.ass";
	{
		std::ofstream out(file);
		out << "[Script Info]\n"
			<< "Title: Get Entries Test\n"
			<< "ScriptType: v4.00+\n\n"
			<< "[Events]\n"
			<< "Format: Layer, Start, End, Style, Text\n"
			<< "Dialogue: 0,0:00:01.00,0:00:03.00,Default,,0,0,0,First line\n"
			<< "Dialogue: 0,0:00:03.00,0:00:05.00,Default,,0,0,0,{\\b1}Bold text\n";
	}

	ASSA assa;
	assa.reading_from_file(file);

	const Vector< Subtitle_data >& entries = assa.getting_entries();
	EXPECT_EQ(entries.get_size(), 2);
	EXPECT_EQ(entries[0].text, "First line");
	EXPECT_EQ(entries[1].text, "{\\b1}Bold text");

	std::remove(file.c_str());
}

TEST(ASSATest, reading_from_file)
{
	std::ofstream out("test.ass");
	out << "[Script Info]\n"
		<< "Title: Default Test\n"
		<< "ScriptType: v4.00+\n\n"
		<< "[Events]\n"
		<< "Format: Layer, Start, End, Style, Text\n"
		<< "Dialogue: 0,0:00:01.00,0:00:03.00,Default,,0,0,0,First line\n"
		<< "Dialogue: 0,0:00:03.00,0:00:05.00,Default,,0,0,0,Second line\n";
	out.close();

	ASSA assa;
	EXPECT_NO_THROW(assa.reading_from_file("test.ass"));
	EXPECT_EQ(assa.getting_entries().get_size(), 2);

	std::remove("test.ass");
}

TEST(ASSATest, writing_to_file)
{
	std::ofstream out("test.ass");
	out << "[Script Info]\n"
		<< "Title: Default Test\n"
		<< "ScriptType: v4.00+\n\n"
		<< "[Events]\n"
		<< "Format: Layer, Start, End, Style, Text\n"
		<< "Dialogue: 0,0:00:01.00,0:00:03.00,Default,,0,0,0,First line\n"
		<< "Dialogue: 0,0:00:03.00,0:00:05.00,Default,,0,0,0,Second line\n";
	out.close();
	ASSA assa;
	assa.reading_from_file("test.ass");

	EXPECT_NO_THROW(assa.writing_to_file("output.ass"));

	ASSA assa2;
	EXPECT_NO_THROW(assa2.reading_from_file("output.ass"));
	EXPECT_EQ(assa2.getting_entries().get_size(), 2);

	std::remove("output.ass");
}

TEST(SBVTest, ValidFile)
{
	std::ofstream out("test.sbv");
	out << "0:00:01.000,0:00:03.500\n";
	out << "First line of subtitle\n\n";
	out << "0:00:03.000,0:00:04.000\n";
	out << "Second line, overlapping\n\n";
	out.close();

	SBV sbv;
	EXPECT_TRUE(sbv.is_file_valid("test.sbv"));
}

TEST(SBVTest, InvalidFile)
{
	SBV sbv;
	EXPECT_FALSE(sbv.is_file_valid("does_not_exist.sbv"));

	std::ofstream out("bad.sbv");
	out << "Just text without timing\n";
	out.close();
	EXPECT_FALSE(sbv.is_file_valid("bad.sbv"));
}

TEST(SBVTest, ReadWrite)
{
	std::ofstream out("in.sbv");
	out << "0:00:01.000,0:00:03.500\n";
	out << "First subtitle\n\n";
	out << "0:00:04.000,0:00:05.500\n";
	out << "Second subtitle\n\n";
	out.close();

	SBV sbv;
	sbv.reading_from_file("in.sbv");
	sbv.writing_to_file("out.sbv");
	EXPECT_TRUE(sbv.is_file_valid("out.sbv"));
}

TEST(SBVTest, ShiftingTime)
{
	std::ofstream out("shift_in.sbv");
	out << "0:00:01.000,0:00:03.500\n";
	out << "First subtitle\n\n";
	out << "0:00:03.500,0:00:05.000\n";
	out << "Second subtitle\n\n";
	out.close();

	SBV sbv;
	sbv.reading_from_file("shift_in.sbv");
	sbv.shifting_time(1000, true, true);
	const Subtitle_data& e0 = sbv.getting_entries()[0];
	EXPECT_EQ(e0.start_time, 1000 + 1000);
	EXPECT_EQ(e0.end_time, 3500 + 1000);
}

TEST(SBVTest, FindingCollisions)
{
	std::ofstream out("collide.sbv");
	out << "0:00:01.000,0:00:03.500\n";
	out << "First subtitle\n\n";
	out << "0:00:02.500,0:00:04.000\n";
	out << "Second overlapping subtitle\n\n";
	out.close();

	SBV sbv;
	sbv.reading_from_file("collide.sbv");
	Vector< Subtitle_data > cols = sbv.finding_collisions();
	EXPECT_EQ(cols.get_size(), 2);
}

TEST(SBVTest, RemovingStyling)
{
	std::ofstream out("sty.sbv");
	out << "0:00:00.000,0:00:01.000\n";
	out << "<b>Bold</b> and <i>Italic</i>\n\n";
	out.close();

	SBV sbv;
	sbv.reading_from_file("sty.sbv");
	sbv.removing_styling();

	const std::string& txt = sbv.getting_entries()[0].text;
	EXPECT_EQ(txt, "Bold and Italic");
}
TEST(SBVTest, ApplyDefaultStyle)
{
	std::ofstream out("defstyle.sbv");
	out << "0:00:01.000,0:00:03.500\n";
	out << "First line of subtitle with <b>bold</b> text\n\n";
	out << "0:00:04.000,0:00:05.500\n";
	out << "Second line with <i>italic</i> text\n\n";
	out.close();

	SBV sbv;
	sbv.reading_from_file("defstyle.sbv");

	sbv.apply_default_style();

	const std::string& txt1 = sbv.getting_entries()[0].text;
	const std::string& txt2 = sbv.getting_entries()[1].text;

	EXPECT_EQ(txt1.find("<"), std::string::npos);
	EXPECT_EQ(txt2.find("<"), std::string::npos);
}

TEST(SBVTest, AddEntryAndGetEntries)
{
	std::ofstream out("entries.sbv");
	out << "0:00:01.000,0:00:03.500\n";
	out << "First line\n\n";
	out << "0:00:04.000,0:00:05.500\n";
	out << "Second line\n\n";
	out.close();

	SBV sbv;
	sbv.reading_from_file("entries.sbv");

	Subtitle_data ne{ 99, "New subtitle", 5000, 6000 };
	sbv.add_entry(ne);

	const Vector< Subtitle_data >& all = sbv.getting_entries();
	EXPECT_EQ(all.get_size(), 3u);
	EXPECT_EQ(all[2].text, "New subtitle");
}

TEST(LRCTest, ValidFile)
{
	std::ofstream out("test.lrc");
	out << "[00:01.00]First line\n";
	out << "[00:03.50]Second line\n";
	out.close();

	LRC lrc;
	EXPECT_TRUE(lrc.is_file_valid("test.lrc"));
}

TEST(LRCTest, InvalidFile)
{
	LRC lrc;
	EXPECT_FALSE(lrc.is_file_valid("invalid.lrc"));
}

TEST(LRCTest, ReadWrite)
{
	std::ofstream out("test.lrc");
	out << "[00:01.00]First line\n";
	out << "[00:03.50]Second line\n";
	out.close();

	LRC lrc;
	lrc.reading_from_file("test.lrc");
	lrc.writing_to_file("output.lrc");
	EXPECT_TRUE(lrc.is_file_valid("output.lrc"));
}

TEST(LRCTest, ShiftingTime)
{
	std::ofstream out("test.lrc");
	out << "[00:01.00]First line\n";
	out << "[00:03.50]Second line\n";
	out.close();

	LRC lrc;
	lrc.reading_from_file("test.lrc");
	lrc.shifting_time(1000, true, true);
	lrc.writing_to_file("shifted.lrc");
	EXPECT_TRUE(lrc.is_file_valid("shifted.lrc"));
}

TEST(LRCTest, FindingCollisions)
{
	std::ofstream out("test.lrc");
	out << "[00:01.00]First line\n";
	out << "[00:01.50]Second line (collision)\n";
	out << "[00:03.00]No collision\n";
	out.close();

	LRC lrc;
	lrc.reading_from_file("test.lrc");

	Vector< Subtitle_data > collisions = lrc.finding_collisions();
	EXPECT_EQ(collisions.get_size(), 2);

	std::remove("test.lrc");
}

TEST(LRCTest, RemovingStyling)
{
	std::ofstream out("test.lrc");
	out << "[00:01.00]First line with <b>bold</b> text\n";
	out << "[00:03.50]Second line with <i>italic</i> text\n";
	out.close();

	LRC lrc;
	lrc.reading_from_file("test.lrc");
	lrc.removing_styling();

	const std::string& txt1 = lrc.getting_entries()[0].text;
	const std::string& txt2 = lrc.getting_entries()[1].text;

	EXPECT_EQ(txt1, "First line with bold text");
	EXPECT_EQ(txt2, "Second line with italic text");
}

TEST(LRCTest, ApplyDefaultStyle)
{
	std::ofstream out("test.lrc");
	out << "[00:01.00]First line with <b>bold</b> text\n";
	out << "[00:03.50]Second line with <i>italic</i> text\n";
	out.close();

	LRC lrc;
	lrc.reading_from_file("test.lrc");
	lrc.apply_default_style();

	const std::string& txt1 = lrc.getting_entries()[0].text;
	const std::string& txt2 = lrc.getting_entries()[1].text;

	EXPECT_EQ(txt1.find("<"), std::string::npos);
	EXPECT_EQ(txt2.find("<"), std::string::npos);
}

TEST(LRCTest, AddEntry)
{
	std::ofstream out("test.lrc");
	out << "[00:01.00]First line\n";
	out << "[00:03.50]Second line\n";
	out.close();

	LRC lrc;
	lrc.reading_from_file("test.lrc");

	Subtitle_data new_entry = { 99, "New subtitle", 5000, 6000 };
	lrc.add_entry(new_entry);

	const Vector< Subtitle_data >& all = lrc.getting_entries();
	EXPECT_EQ(all.get_size(), 3u);
	EXPECT_EQ(all[2].text, "New subtitle");
}

TEST(LRCTest, GetEntries)
{
	std::ofstream out("test.lrc");
	out << "[00:01.00]First line\n";
	out << "[00:03.50]Second line\n";
	out.close();

	LRC lrc;
	lrc.reading_from_file("test.lrc");

	const Vector< Subtitle_data >& entries = lrc.getting_entries();
	EXPECT_EQ(entries.get_size(), 2);
	EXPECT_EQ(entries[0].text, "First line");
	EXPECT_EQ(entries[1].text, "Second line");
}

TEST(LRCTest, reading_from_file)
{
	std::ofstream out("test.lrc");
	out << "[00:01.00]First line\n";
	out << "[00:03.50]Second line\n";
	out.close();

	LRC lrc;
	lrc.reading_from_file("test.lrc");
	EXPECT_TRUE(lrc.is_file_valid("test.lrc"));
}

TEST(LRCTest, writing_to_file)
{
	std::ofstream out("test.lrc");
	out << "[00:01.00]First line\n";
	out << "[00:03.50]Second line\n";
	out.close();

	LRC lrc;
	lrc.reading_from_file("test.lrc");
	lrc.writing_to_file("output.lrc");
	EXPECT_TRUE(lrc.is_file_valid("output.lrc"));
}

int main(int argc, char** argv)
{
	testing::InitGoogleTest(&argc, argv);
	return RUN_ALL_TESTS();
}
