#include <math.h>
#include <stdio.h>
#include <stdlib.h>

void printing_the_matrix(double *the_matrix, int the_lines, int the_columns)
{
	for (int i = 0; i < the_lines; i++)
	{
		for (int j = 0; j < the_columns; j++)
		{
			printf("%g ", the_matrix[i * the_columns + j]);
		}
		printf("\n");
	}
}

double *locate_the_matrix(int the_columns, int the_lines)
{
	return malloc(the_lines * the_columns * sizeof(float));
}

int the_gauss_invert_method(double *the_matrix, double *the_invert_matrix, int the_number_of_iterations)
{
	for (int i = 0; i < the_number_of_iterations; i++)
	{
		for (int j = 0; j < the_number_of_iterations; j++)
		{
			if (i == j)
			{
				the_invert_matrix[i * the_number_of_iterations + j] = 1.0;
			}
			else
			{
				the_invert_matrix[i * the_number_of_iterations + j] = 0.0;
			}
		}
	}
	for (int i = 0; i < the_number_of_iterations; i++)
	{
		double the_max_element = fabs(the_matrix[i * the_number_of_iterations + i]);
		int the_max_line = i;

		for (int k = i + 1; k < the_number_of_iterations; k++)
		{
			if (fabs(the_matrix[k * the_number_of_iterations + i]) > the_max_element)
			{
				the_max_element = fabs(the_matrix[k * the_number_of_iterations + i]);
				the_max_line = k;
			}
		}

		if (the_max_element == 0)
		{
			return 1;
		}

		for (int k = 0; k < the_number_of_iterations; k++)
		{
			double the_temp_value = the_matrix[the_max_line * the_number_of_iterations + k];
			the_matrix[the_max_line * the_number_of_iterations + k] = the_matrix[i * the_number_of_iterations + k];
			the_matrix[i * the_number_of_iterations + k] = the_temp_value;

			the_temp_value = the_invert_matrix[the_max_line * the_number_of_iterations + k];
			the_invert_matrix[the_max_line * the_number_of_iterations + k] = the_invert_matrix[i * the_number_of_iterations + k];
			the_invert_matrix[i * the_number_of_iterations + k] = the_temp_value;
		}

		double pivot = the_matrix[i * the_number_of_iterations + i];
		for (int k = 0; k < the_number_of_iterations; k++)
		{
			the_matrix[i * the_number_of_iterations + k] /= pivot;
			the_invert_matrix[i * the_number_of_iterations + k] /= pivot;
		}

		for (int k = 0; k < the_number_of_iterations; k++)
		{
			if (k != i)
			{
				double the_multiplier = the_matrix[k * the_number_of_iterations + i];

				for (int j = 0; j < the_number_of_iterations; j++)
				{
					the_matrix[k * the_number_of_iterations + j] -= the_multiplier * the_matrix[i * the_number_of_iterations + j];
					the_invert_matrix[k * the_number_of_iterations + j] -=
						the_multiplier * the_invert_matrix[i * the_number_of_iterations + j];
				}
			}
		}
	}
	return 0;
}

int main(int argc, const char *argv[])
{
	FILE *the_input_value = fopen(argv[1], "r");

	if (!the_input_value)
	{
		fprintf(stderr, "Error: can't open file\n");
		return 1;
	}

	int R, C;
	int the_size_of_matrix = fscanf(the_input_value, "%d %d", &R, &C);
	if (fscanf(the_input_value, "%d %d", &R, &C) != 2 || R <= 0 || C <= 0)
	{
		fprintf(stderr, "Error: wrong matrix size\n");
		fclose(the_input_value);
		return 1;
	}

	if (R != C)
	{
		fprintf(stderr, "Error: the matrix size isn't square\n");
		fclose(the_input_value);
		return 1;
	}

	int the_number_of_iterations;
	fscanf(the_input_value, "%d", &the_number_of_iterations);
	double *the_matrix = locate_the_matrix(the_number_of_iterations, the_number_of_iterations);
	double *the_invert_matrix = locate_the_matrix(the_number_of_iterations, the_number_of_iterations);

	for (int i = 0; i < the_number_of_iterations; i++)
	{
		for (int j = 0; j < the_number_of_iterations; j++)
		{
			if (fscanf(the_input_value, "%lf", &the_matrix[i * the_number_of_iterations + j]) != 1)
			{
				fprintf(stderr, "Error: failed to read matrix element\n");
				free(the_matrix);
				free(the_invert_matrix);
				fclose(the_input_value);
				return 1;
			}
		}
	}

	if (the_gauss_invert_method(the_matrix, the_invert_matrix, the_number_of_iterations) == 1)
	{
		printf("No solutions: the matrix is degenerate\n");
	}
	else
	{
		printing_the_matrix(the_invert_matrix, the_number_of_iterations, the_number_of_iterations);
	}

	free(the_matrix);
	free(the_invert_matrix);
    fclose(the_input_value);
    
	return 0;
}
