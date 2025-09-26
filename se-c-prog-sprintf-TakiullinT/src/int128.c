#include "int128.h"

#include <ctype.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

int128_t int128_add(int128_t a, uint64_t b)
{
	int128_t result;
	uint64_t carry = 0;

	result.low = a.low + b;
	carry = (result.low < a.low) ? 1 : 0;
	result.high = a.high + carry;

	return result;
}

int128_t int128_div(int128_t num, uint64_t base, uint64_t *remainder)
{
	int128_t result = { 0, 0 };

	uint64_t temp = 0;

	for (int i = 127; i >= 0; i--)
	{
		temp = (temp << 1) | ((num.high >> 63) & 1);
		num.high = (num.high << 1) | (num.low >> 63);
		num.low <<= 1;
		if (temp >= base)
		{
			temp -= base;
			if (i < 64)
			{
				result.low |= 1ULL << i;
			}
			else
			{
				result.high |= 1ULL << (i - 64);
			}
		}
	}

	*remainder = temp;
	return result;
}

int128_t int128_mul(int128_t a, uint64_t b)
{
	int128_t result = { 0, 0 };
	uint64_t low_high = (a.low >> 32) * (b & 0xFFFFFFFF);
	uint64_t high_low = (b >> 32) * (a.low & 0xFFFFFFFF);
	uint64_t low_low = (a.low & 0xFFFFFFFF) * (b & 0xFFFFFFFF);

	result.low = low_low + ((low_high & 0xFFFFFFFF) << 32);
	result.high = a.high * b + (low_high >> 32) + (high_low >> 32);

	if (result.low < low_low)
	{
		result.high++;
	}

	return result;
}

void int128_to_str(int128_t number, char *buffer, int base, int uppercase)
{
	const char *digits = uppercase ? "0123456789ABCDEF" : "0123456789abcdef";

	if (number.high == 0 && number.low == 0)
	{
		buffer[0] = '0';
		buffer[1] = '\0';
		return;
	}

	int is_negative = (number.high & (1ULL << 63)) != 0;
	if (is_negative)
	{
		number.low = ~number.low + 1;
		number.high = ~number.high;
		if (number.low == 0)
		{
			number.high++;
		}
	}

	int i = 0;
	while (number.high > 0 || number.low > 0)
	{
		uint64_t remainder = 0;
		number = int128_div(number, base, &remainder);
		buffer[i++] = digits[remainder];
	}

	buffer[i] = '\0';

	for (int j = 0; j < i / 2; j++)
	{
		char temp = buffer[j];
		buffer[j] = buffer[i - j - 1];
		buffer[i - j - 1] = temp;
	}
}

int int128_from_string(const char *number, int128_t *result)
{
	int128_t value = { 0, 0 };
	int base = 10;
	int is_negative = 0;

	if (number == NULL || *number == '\0')
	{
		fprintf(stderr, "Error: Empty number string.\n");
		return 1;
	}

	while (isspace(*number))
		number++;

	if (*number == '-')
	{
		is_negative = 1;
		number++;
	}
	else if (*number == '+')
	{
		number++;
	}

	if (strncmp(number, "0x", 2) == 0 || strncmp(number, "0X", 2) == 0)
	{
		base = 16;
		number += 2;
	}
	else if (strncmp(number, "0b", 2) == 0 || strncmp(number, "0B", 2) == 0)
	{
		base = 2;
		number += 2;
	}
	else if (*number == '0' && isdigit(*(number + 1)))
	{
		base = 8;
		number += 1;
	}

	if (*number == '\0')
	{
		fprintf(stderr, "Error: Invalid number representation.\n");
		return 1;
	}

	while (*number)
	{
		int digit_value;
		char digit = tolower(*number);

		if (isdigit(digit))
		{
			digit_value = digit - '0';
		}
		else if (base == 16 && isxdigit(digit))
		{
			digit_value = digit - 'a' + 10;
		}
		else
		{
			fprintf(stderr, "Error: Invalid digit '%c' for base %d.\n", digit, base);
			return 1;
		}

		if (digit_value >= base)
		{
			fprintf(stderr, "Error: Digit '%c' exceeds base %d.\n", digit, base);
			return 1;
		}

		int128_t temp = int128_mul(value, base);
		value = int128_add(temp, digit_value);
		number++;
	}

	if (is_negative)
	{
		value.low = ~value.low + 1;
		value.high = ~value.high;
		if (value.low == 0)
		{
			value.high += 1;
		}
	}

	*result = value;
	return 0;
}
