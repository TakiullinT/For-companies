#include "convert.h"

#include <png.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

int convert(const char *input_file, const char *output_file)
{
	FILE *fp = fopen(input_file, "rb");
	if (!fp)
	{
		fprintf(stderr, "Error: Failed to open input file\n");
		return 1;
	}

	unsigned char header[8];
	fread(header, 1, 8, fp);
	if (png_sig_cmp(header, 0, 8))
	{
		fclose(fp);
		fprintf(stderr, "Error: Input file is not a valid PNG\n");
		return 1;
	}

	png_structp png = png_create_read_struct(PNG_LIBPNG_VER_STRING, NULL, NULL, NULL);
	if (!png)
	{
		fclose(fp);
		fprintf(stderr, "Error: Failed to create PNG read structure\n");
		return 1;
	}

	png_infop info = png_create_info_struct(png);
	if (!info)
	{
		png_destroy_read_struct(&png, NULL, NULL);
		fclose(fp);
		fprintf(stderr, "Error: Failed to create PNG info structure\n");
		return 1;
	}

	if (setjmp(png_jmpbuf(png)))
	{
		png_destroy_read_struct(&png, &info, NULL);
		fclose(fp);
		fprintf(stderr, "Error: Error during PNG read\n");
		return 1;
	}

	png_init_io(png, fp);
	png_set_sig_bytes(png, 8);
	png_read_info(png, info);

	png_byte color_type = png_get_color_type(png, info);
	png_byte bit_depth = png_get_bit_depth(png, info);

	png_set_strip_alpha(png);

	if (bit_depth != 8)
	{
		png_destroy_read_struct(&png, &info, NULL);
		fclose(fp);
		fprintf(stderr, "Error: Only 8-bit per channel images are supported\n");
		return 1;
	}

	if (color_type != PNG_COLOR_TYPE_GRAY && color_type != PNG_COLOR_TYPE_RGB && color_type != PNG_COLOR_TYPE_PALETTE)
	{
		png_destroy_read_struct(&png, &info, NULL);
		fclose(fp);
		fprintf(stderr, "Error: Unsupported color type. Only GRAY, RGB, and PALETTE are supported\n");
		return 1;
	}

	if (color_type == PNG_COLOR_TYPE_GRAY)
	{
		png_set_gray_to_rgb(png);
	}
	if (color_type == PNG_COLOR_TYPE_GRAY && bit_depth < 8)
	{
		png_set_expand_gray_1_2_4_to_8(png);
	}
	if (color_type == PNG_COLOR_TYPE_RGB)
	{
		png_set_rgb_to_gray_fixed(png, 1, -1, -1);
	}
	if (color_type == PNG_COLOR_TYPE_PALETTE)
	{
		png_set_palette_to_rgb(png);
	}

	if (png_get_valid(png, info, PNG_INFO_tRNS))
	{
		png_set_tRNS_to_alpha(png);
	}
	png_read_update_info(png, info);

	FILE *out_fp = fopen(output_file, "wb");
	if (!out_fp)
	{
		png_destroy_read_struct(&png, &info, NULL);
		fclose(fp);
		fprintf(stderr, "Error: Failed to open output file\n");
		return 1;
	}

	png_structp png_out = png_create_write_struct(PNG_LIBPNG_VER_STRING, NULL, NULL, NULL);
	if (!png_out)
	{
		png_destroy_read_struct(&png, &info, NULL);
		fclose(fp);
		fclose(out_fp);
		fprintf(stderr, "Error: Failed to create PNG write structure\n");
		return 1;
	}

	png_infop info_out = png_create_info_struct(png_out);
	if (!info_out)
	{
		png_destroy_read_struct(&png, &info, NULL);
		png_destroy_write_struct(&png_out, NULL);
		fclose(fp);
		fclose(out_fp);
		fprintf(stderr, "Error: Failed to create PNG write info structure\n");
		return 1;
	}

	if (setjmp(png_jmpbuf(png_out)))
	{
		png_destroy_read_struct(&png, &info, NULL);
		png_destroy_write_struct(&png_out, &info_out);
		fclose(fp);
		fclose(out_fp);
		fprintf(stderr, "Error: Error during PNG write\n");
		return 1;
	}

	png_init_io(png_out, out_fp);

	png_set_IHDR(png_out, info_out, png_get_image_width(png, info), png_get_image_height(png, info), 8, png_get_color_type(png, info), PNG_INTERLACE_NONE, PNG_COMPRESSION_TYPE_DEFAULT, PNG_FILTER_TYPE_DEFAULT);

	png_write_info(png_out, info_out);

	png_bytep *row_pointers = (png_bytep *)malloc(sizeof(png_bytep) * png_get_image_height(png, info));
	if (!row_pointers)
	{
		fprintf(stderr, "Error: Memory allocation failed\n");
		png_destroy_read_struct(&png, &info, NULL);
		png_destroy_write_struct(&png_out, &info_out);
		fclose(fp);
		fclose(out_fp);
		return 1;
	}

	for (int y = 0; y < png_get_image_height(png, info); y++)
	{
		row_pointers[y] = (png_bytep)malloc(png_get_rowbytes(png, info));
		if (!row_pointers[y])
		{
			fprintf(stderr, "Error: Memory allocation failed\n");
			for (int i = 0; i < y; i++)
				free(row_pointers[i]);
			free(row_pointers);
			png_destroy_read_struct(&png, &info, NULL);
			png_destroy_write_struct(&png_out, &info_out);
			fclose(fp);
			fclose(out_fp);
			return 1;
		}
	}

	png_read_image(png, row_pointers);

	png_write_image(png_out, row_pointers);

	png_write_end(png_out, NULL);

	for (int y = 0; y < png_get_image_height(png, info); y++)
	{
		free(row_pointers[y]);
	}
	free(row_pointers);

	png_destroy_read_struct(&png, &info, NULL);
	png_destroy_write_struct(&png_out, &info_out);

	fclose(fp);
	fclose(out_fp);

	return 0;
}
