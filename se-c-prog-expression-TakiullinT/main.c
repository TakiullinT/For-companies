#include <ctype.h>
#include <limits.h>
#include <math.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#define MAX_EXPR_LEN 1024
#define MAX_RPN_LEN 1024
#define STACK_SIZE 256

void normalize_expression(char *expression)
{
	char *src = expression;
	char *dst = expression;

	while (*src)
	{
		if ((unsigned char)src[0] == 0xE2 && (unsigned char)src[1] == 0x88 && (unsigned char)src[2] == 0x92)
		{
			*dst++ = '-';
			src += 3;
		}
		else
		{
			*dst++ = *src++;
		}
	}
	*dst = '\0';
}
int int_pow(int base, int exponent)
{
	int result = 1;
	for (int i = 0; i < exponent; i++)
	{
		if (result > INT_MAX / base)
		{
			fprintf(stderr, "Error: Integer overflow during power operation.\n");
			return 0;
		}
		result *= base;
	}
	return result;
}

typedef struct
{
	int data[STACK_SIZE];
	int top;
} the_int_stack;

typedef struct
{
	char data[STACK_SIZE];
	int top;
} the_char_stack;

void init_int_stack(the_int_stack *stack)
{
	stack->top = -1;
}

int is_int_stack_empty(the_int_stack *stack)
{
	return stack->top == -1;
}

int push_int_stack(the_int_stack *stack, int value)
{
	if (stack->top >= STACK_SIZE - 1)
		return 0;
	stack->data[++stack->top] = value;
	return 1;
}

int pop_int_stack(the_int_stack *stack, int *value)
{
	if (is_int_stack_empty(stack))
		return 0;
	*value = stack->data[stack->top--];
	return 1;
}

void init_char_stack(the_char_stack *stack)
{
	stack->top = -1;
}

int is_char_stack_empty(the_char_stack *stack)
{
	return stack->top == -1;
}

int push_char_stack(the_char_stack *stack, char value)
{
	if (stack->top >= STACK_SIZE - 1)
		return 0;
	stack->data[++stack->top] = value;
	return 1;
}

int pop_char_stack(the_char_stack *stack, char *value)
{
	if (is_char_stack_empty(stack))
		return 0;
	*value = stack->data[stack->top--];
	return 1;
}

int peek_char_stack(the_char_stack *stack, char *value)
{
	if (is_char_stack_empty(stack))
		return 0;
	*value = stack->data[stack->top];
	return 1;
}

int is_operator(const char *math_expr, size_t index)
{
	if (math_expr[index] == '*' && math_expr[index + 1] == '*')
		return 1;
	if ((math_expr[index] == '<' && math_expr[index + 1] == '<') || (math_expr[index] == '>' && math_expr[index + 1] == '>'))
		return 1;
	return math_expr[index] == '+' || math_expr[index] == '-' || math_expr[index] == '*' || math_expr[index] == '/' ||
		   math_expr[index] == '%' || math_expr[index] == '^' || math_expr[index] == '|' || math_expr[index] == '&' ||
		   math_expr[index] == '~' || math_expr[index] == '>' || math_expr[index] == '<' || math_expr[index] == '(' ||
		   math_expr[index] == ')';
}
int priority(char operand)
{
	switch (operand)
	{
	case '+':
	case '-':
		return 6;
	case '*':
	case '/':
	case '%':
		return 8;
	case 'P':
		return 9;
	case '~':
		return 10;
	case '&':
		return 5;
	case '|':
	case '^':
		return 4;
	case 'R':
		return 7;
	case 'L':
		return 3;
	default:
		return 0;
	}
}

int is_right_associative(char operand)
{
	return operand == '^' | operand == '~';
}

char *convert_to_rpn(const char *math_expression, int *error_code)
{
	static char rpn[MAX_RPN_LEN] = { 0 };
	int rpn_index = 0;
	the_char_stack operator_stack;
	init_char_stack(&operator_stack);

	int prev_was_operator = 1;

	for (size_t i = 0; math_expression[i] != '\0'; i++)
	{
		char current = math_expression[i];

		if (isspace(current))
		{
			continue;
		}
		else if (isdigit(current) || (prev_was_operator && (current == '+' || current == '-')))
		{
			int sign = 1;
			if ((current == '+' || current == '-') && prev_was_operator)
			{
				if (current == '-')
				{
					sign = -1;
				}
				i++;
				while (isspace(math_expression[i]))
					i++;
			}

			int num = 0;
			while (isdigit(math_expression[i]))
			{
				num = num * 10 + (math_expression[i++] - '0');
			}
			num *= sign;

			rpn_index += sprintf(&rpn[rpn_index], "%d ", num);
			i--;
			prev_was_operator = 0;
		}
		else if ((current == '+' || current == '-') && prev_was_operator && math_expression[i + 1] == '(')
		{
			if (current == '-')
			{
				rpn_index += sprintf(&rpn[rpn_index], "0 ");
				push_char_stack(&operator_stack, '-');
			}
			prev_was_operator = 1;
		}
		else if (current == '(')
		{
			push_char_stack(&operator_stack, current);
			prev_was_operator = 1;
		}
		else if (current == ')')
		{
			char op;
			while (pop_char_stack(&operator_stack, &op) && op != '(')
			{
				rpn[rpn_index++] = op;
				rpn[rpn_index++] = ' ';
			}
			if (op != '(')
			{
				*error_code = 3;
				return NULL;
			}
			prev_was_operator = 0;
		}
		else if (current == '*' && math_expression[i + 1] == '*')
		{
			rpn[rpn_index++] = 'P';
			rpn[rpn_index++] = ' ';
			prev_was_operator = 1;
		}
		else if (current == '<' && math_expression[i + 1] == '<')
		{
			push_char_stack(&operator_stack, 'L');
			i++;
		}
		else if (current == '>' && math_expression[i + 1] == '>')
		{
			push_char_stack(&operator_stack, 'R');
			i++;
		}
		else if (is_operator(math_expression, i))
		{
			char operand;
			while (peek_char_stack(&operator_stack, &operand) && priority(operand) >= priority(current) &&
				   (!is_right_associative(current) || priority(operand) > priority(current)))
			{
				pop_char_stack(&operator_stack, &operand);
				rpn[rpn_index++] = operand;
				rpn[rpn_index++] = ' ';
			}
			push_char_stack(&operator_stack, current);
			prev_was_operator = 1;
		}
		else
		{
			*error_code = 1;
			return NULL;
		}
	}

	char operand;
	while (pop_char_stack(&operator_stack, &operand))
	{
		if (operand == '(' || operand == ')')
		{
			*error_code = 3;
			return NULL;
		}
		rpn[rpn_index++] = operand;
		rpn[rpn_index++] = ' ';
	}

	rpn[rpn_index] = '\0';
	return rpn;
}

int evaluate_rpn(const char *rpn, int *error_code)
{
	the_int_stack stack;
	init_int_stack(&stack);

	for (size_t i = 0; rpn[i] != '\0'; i++)
	{
		char current = rpn[i];

		if (isspace(current))
		{
			continue;
		}
		else if (isdigit(current) || (current == '-' && isdigit(rpn[i + 1])))
		{
			int num = 0, sign = 1;
			if (current == '-')
			{
				sign = -1;
				i++;
			}
			while (isdigit(rpn[i]))
			{
				num = num * 10 + (rpn[i++] - '0');
			}
			push_int_stack(&stack, sign * num);
			i--;
		}
		else if (current == 'R')
		{
			int b, a;
			if (!pop_int_stack(&stack, &b) || !pop_int_stack(&stack, &a))
			{
				*error_code = 3;
				return 0;
			}
			if (b < 0 || b >= (sizeof(int) * CHAR_BIT))
			{
				*error_code = 2;
				return 0;
			}
			push_int_stack(&stack, a >> b);
		}
		else if (current == 'L')
		{
			int b, a;
			if (!pop_int_stack(&stack, &b) || !pop_int_stack(&stack, &a))
			{
				*error_code = 3;
				return 0;
			}
			if (b < 0 || b >= (sizeof(int) * CHAR_BIT))
			{
				*error_code = 2;
				return 0;
			}
			push_int_stack(&stack, a << b);
		}

		else if (is_operator(rpn, i))
		{
			int b, a, result;
			if (current == '~')
			{
				if (!pop_int_stack(&stack, &a))
				{
					*error_code = 3;
					return 0;
				}
				result = ~a;
				push_int_stack(&stack, result);
			}
			else
			{
				if (!pop_int_stack(&stack, &b) || !pop_int_stack(&stack, &a))
				{
					*error_code = 3;
					return 0;
				}

				if (current == '+')
				{
					result = a + b;
				}
				else if (current == '-')
				{
					result = a - b;
				}
				else if (current == '*')
				{
					result = a * b;
				}
				else if (current == '/')
				{
					if (b == -1 && a == INT_MIN)
					{
						*error_code = 2;
						return 0;
					}
					if (b == 0)
					{
						*error_code = 2;
						return 0;
					}
					result = a / b;
				}
				else if (current == '%')
				{
					if (b == -1 && a == INT_MIN)
					{
						*error_code = 2;
						return 0;
					}
					if (b == 0)
					{
						*error_code = 2;
						return 0;
					}
					result = a % b;
				}
				else if (current == '^')
				{
					result = a ^ b;
				}
				else if (current == '&')
				{
					result = a & b;
				}
				else if (current == '|')
				{
					result = a | b;
				}
				else if (current == 'L')
				{
					result = a << b;
				}
				else if (current == 'R')
				{
					if (b < 0 || b >= (sizeof(int) * CHAR_BIT))
					{
						*error_code = 2;
						return 0;
					}
					result = a >> b;
				}
				else if (current == 'P')
				{
					result = int_pow(a, b);
				}
				else
				{
					*error_code = 1;
					return 0;
				}

				push_int_stack(&stack, result);
			}
		}
	}

	int result;
	if (!pop_int_stack(&stack, &result) || !is_int_stack_empty(&stack))
	{
		*error_code = 3;
		return 0;
	}

	return result;
}
int main(int argc, char *argv[])
{
	if (argc != 2)
	{
		fprintf(stderr, "Usage: %s \"expression\"\n", argv[0]);
		return 4;
	}

	char expression[MAX_EXPR_LEN];
	strncpy(expression, argv[1], MAX_EXPR_LEN - 1);
	expression[MAX_EXPR_LEN - 1] = '\0';

	normalize_expression(expression);

	int error_code = 0;
	char *rpn = convert_to_rpn(expression, &error_code);
	if (!rpn)
	{
		fprintf(stderr, "Error: Unable to parse expression (code %d).\n", error_code);
		return error_code;
	}

	int result = evaluate_rpn(rpn, &error_code);
	if (error_code != 0)
	{
		fprintf(stderr, "Error: Evaluation failed (code %d).\n", error_code);
		return error_code;
	}

	printf("%d\n", result);
	return 0;
}
