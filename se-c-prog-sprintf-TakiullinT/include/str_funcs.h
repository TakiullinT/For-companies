#ifndef STR_FUNCS_H
#define STR_FUNCS_H

#include <stddef.h>

int the_strlen(const char *str);
char *the_strchr(const char *str, int c);
int the_strncmp(const char *str1, const char *str2, size_t n);
int the_isspace(char c);
char the_tolower(char c);

#endif
