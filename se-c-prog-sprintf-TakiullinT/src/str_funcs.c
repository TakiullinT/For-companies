#include "str_funcs.h"

#include <ctype.h>

int the_strlen(const char *str)
{
	int length = 0;
	while (*str != '\0')
	{
		length++;
		str++;
	}
	return length;
}

char *the_strchr(const char *str, int c)
{
	while (*str != '\0')
	{
		if (*str == (char)c)
		{
			return (char *)str;
		}
		str++;
	}
	return NULL;
}

int the_strncmp(const char *str1, const char *str2, size_t n)
{
	while (n && *str1 && (*str1 == *str2))
	{
		str1++;
		str2++;
		n--;
	}
	if (n == 0)
	{
		return 0;
	}
	else
	{
		return *(unsigned char *)str1 - *(unsigned char *)str2;
	}
}

int the_isspace(char c)
{
	return (c == ' ' || c == '\t' || c == '\n' || c == '\v' || c == '\f' || c == '\r');
}

char the_tolower(char c)
{
	if (c >= 'A' && c <= 'Z')
	{
		return c + ('a' - 'A');
	}
	return c;
}
