#include "convert.h"

#include <stdio.h>

int main(int argc, char *argv[])
{
	if (argc != 3)
	{
		fprintf(stderr, "Usage: %s <input_png_file> <output_png_file>\n", argv[0]);
		return 1;
	}

	int result = convert(argv[1], argv[2]);
	return result;
}
