#ifndef INT128_H
#define INT128_H

#include <stdint.h>

typedef struct
{
	uint64_t low;
	uint64_t high;
} int128_t;

int128_t int128_add(int128_t a, uint64_t b);
int128_t int128_mul(int128_t a, uint64_t b);
int128_t int128_div(int128_t num, uint64_t base, uint64_t *remainder);
void int128_to_str(int128_t number, char *buffer, int base, int uppercase);
int int128_from_string(const char *number, int128_t *result);

#endif
