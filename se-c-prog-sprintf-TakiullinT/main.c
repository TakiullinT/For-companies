#include "format.h"

#include <stdio.h>

int main(int argc, char *argv[])
{
	if (argc != 3)
	{
		fprintf(stderr, "Error: Invalid arguments.\n");
		return 1;
	}

	char out_buf[256];
	if (print(out_buf, argv[1], argv[2]) == 0)
	{
		puts(out_buf);
		return 0;
	}
	return 1;
}
