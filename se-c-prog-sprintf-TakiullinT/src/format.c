#include "format.h"

#include <ctype.h>
#include <stdio.h>
#include <string.h>

int format_number(char *out_buf, int128_t number, const char *format)
{
	char temp_buf[256];
	int width = 0;
	char specifier = '\0';
	int add_prefix = 0;
	int force_sign = 0;
	int use_padding = 0;
	int align_left = 0;
	int is_negative = (number.high & (1ULL << 63)) != 0;

	const char *fmt_ptr = format + 1;

	int use_space = 0;
	if (*fmt_ptr == ' ')
	{
		use_space = 1;
		fmt_ptr++;
	}

	if (*fmt_ptr == '+')
	{
		force_sign = 1;
		use_space = 0;
		fmt_ptr++;
	}

	if (*fmt_ptr == '-')
	{
		align_left = 1;
		fmt_ptr++;
	}

	if (*fmt_ptr == '0')
	{
		use_padding = 1;
		fmt_ptr++;
	}

	if (*fmt_ptr == '#')
	{
		add_prefix = 1;
		fmt_ptr++;
	}

	while (*fmt_ptr && !isalpha(*fmt_ptr))
	{
		if (isdigit(*fmt_ptr))
		{
			width = width * 10 + (*fmt_ptr - '0');
		}
		fmt_ptr++;
	}

	if (*fmt_ptr)
	{
		specifier = *fmt_ptr;
	}
	else
	{
		return 1;
	}

	switch (specifier)
	{
	case 'd':
		int128_to_str(number, temp_buf, 10, 0);
		break;
	case 'b':
		int128_to_str(number, temp_buf, 2, 0);
		break;
	case 'o':
		int128_to_str(number, temp_buf, 8, 0);
		break;
	case 'x':
		int128_to_str(number, temp_buf, 16, 0);
		break;
	case 'X':
		int128_to_str(number, temp_buf, 16, 1);
		break;
	default:
		return 1;
	}

	int len = strlen(temp_buf);
	int prefix_len = 0;

	if (add_prefix)
	{
		if (specifier == 'b')
		{
			prefix_len = 2;
		}
		else if (specifier == 'x' || specifier == 'X')
		{
			prefix_len = 2;
		}
		else if (specifier == 'o')
		{
			prefix_len = 1;
		}
	}

	int total_len = len + prefix_len;

	if (is_negative || force_sign || use_space)
	{
		total_len += 1;
	}

	int padding = width - total_len;
	if (padding < 0)
	{
		padding = 0;
	}

	int i = 0;

	if (!align_left && !use_padding)
	{
		for (int j = 0; j < padding; j++)
		{
			out_buf[i++] = ' ';
		}
	}

	if (is_negative)
	{
		out_buf[i++] = '-';
	}
	else if (force_sign)
	{
		out_buf[i++] = '+';
	}
	else if (use_space)
	{
		out_buf[i++] = ' ';
	}

	if (!align_left && use_padding)
	{
		for (int j = 0; j < padding; j++)
		{
			out_buf[i++] = '0';
		}
	}

	if (add_prefix)
	{
		if (specifier == 'b')
		{
			out_buf[i++] = '0';
			out_buf[i++] = 'b';
		}
		else if (specifier == 'x')
		{
			out_buf[i++] = '0';
			out_buf[i++] = 'x';
		}
		else if (specifier == 'X')
		{
			out_buf[i++] = '0';
			out_buf[i++] = 'X';
		}
		else if (specifier == 'o')
		{
			out_buf[i++] = '0';
		}
	}

	for (int j = 0; j < len; j++)
	{
		out_buf[i++] = temp_buf[j];
	}

	if (align_left)
	{
		for (int j = 0; j < padding; j++)
		{
			out_buf[i++] = ' ';
		}
	}

	out_buf[i] = '\0';

	return 0;
}

int print(char *out_buf, const char *format, const char *number)
{
	if (format == NULL || format[0] != '%')
	{
		fprintf(stderr, "Error: Invalid format string.\n");
		return 1;
	}

	int128_t num;
	if (int128_from_string(number, &num) != 0)
	{
		fprintf(stderr, "Error: Invalid number \"%s\".\n", number);
		return 1;
	}

	if (format_number(out_buf, num, format) != 0)
	{
		fprintf(stderr, "Error: Invalid format specifier \"%s\".\n", format);
		return 1;
	}

	return 0;
}
