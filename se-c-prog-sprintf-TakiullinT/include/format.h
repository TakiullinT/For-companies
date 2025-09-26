#ifndef FORMAT_H
#define FORMAT_H

#include "int128.h"

int format_number(char *out_buf, int128_t number, const char *format);
int print(char *out_buf, const char *format, const char *number);

#endif
