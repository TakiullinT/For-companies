#include "quat.h"

#include <cmath>

Quaternion_Math::Quaternion_Math() : a(0), b(0), c(0), d(0) {}

Quaternion_Math::Quaternion_Math(float a, float b, float c, float d) : a(a), b(b), c(c), d(d) {}

Quaternion_Math::Quaternion_Math(float angle_radians, bool is_degrees, const vector3_t& rotation_axis)
{
	float axis_length =
		sqrt(rotation_axis.x * rotation_axis.x + rotation_axis.y * rotation_axis.y + rotation_axis.z * rotation_axis.z);

	if (axis_length == 0.0f)
	{
		if (is_degrees)
		{
			angle_radians = angle_radians * PI / 180.0f;
		}
		a = 1.0f;
		b = c = d = 0.0f;
		return;
	}

	float normalized_x = rotation_axis.x / axis_length;
	float normalized_y = rotation_axis.y / axis_length;
	float normalized_z = rotation_axis.z / axis_length;

	if (is_degrees)
	{
		angle_radians = angle_radians * PI / 180.0f;
	}
	a = cos(angle_radians / 2);
	b = normalized_x * sin(angle_radians / 2);
	c = normalized_y * sin(angle_radians / 2);
	d = normalized_z * sin(angle_radians / 2);
}

bool check_validity(float a, float b, float c, float d) 
{
    if (!std::isfinite(a) && !std::isfinite(b) && !std::isfinite(c) && !std::isfinite(d))
	{
		throw std::overflow_error("Overflow in quaternion addition");
	}
	return 1;
}

Quaternion_Math Quaternion_Math::operator+=(const Quaternion_Math& the_quat)
{
	float new_a = a + the_quat.a;
	float new_b = b + the_quat.b;
	float new_c = c + the_quat.c;
	float new_d = d + the_quat.d;

	check_validity(new_a, new_b, new_c, new_d);

	a = new_a;
	b = new_b;
	c = new_c;
	d = new_d;
	return *this;
}

Quaternion_Math Quaternion_Math::operator+(const Quaternion_Math& the_quat) const
{
	Quaternion_Math the_new_quat = *this; 
	the_new_quat += the_quat;
	return Quaternion_Math(the_new_quat);
}

Quaternion_Math Quaternion_Math::operator-=(const Quaternion_Math& the_quat)
{
	float new_a = a - the_quat.a;
	float new_b = b - the_quat.b;
	float new_c = c - the_quat.c;
	float new_d = d - the_quat.d;

	check_validity(new_a, new_b, new_c, new_d);

	a = new_a;
	b = new_b;
	c = new_c;
	d = new_d;
	return *this;
}

Quaternion_Math Quaternion_Math::operator-(const Quaternion_Math& the_quat) const
{
	Quaternion_Math the_new_quat = *this;
	the_new_quat -= the_quat;
	//check_validity(new_a, new_b, new_c, new_d);
	return Quaternion_Math(the_new_quat);
}

Quaternion_Math Quaternion_Math::operator*(float the_scalar) const
{
	float new_a = a * the_scalar;
	float new_b = b * the_scalar;
	float new_c = c * the_scalar;
	float new_d = d * the_scalar;
	check_validity(new_a, new_b, new_c, new_d);
	return Quaternion_Math(new_a, new_b, new_c, new_d);
}

Quaternion_Math Quaternion_Math::operator*=(float the_scalar)
{
	float new_a = a * the_scalar;
	float new_b = b * the_scalar;
	float new_c = c * the_scalar;
	float new_d = d * the_scalar;

	check_validity(new_a, new_b, new_c, new_d);

	a = new_a;
	b = new_b;
	c = new_c;
	d = new_d;
	return *this;
}
Quaternion_Math Quaternion_Math::operator*(const Quaternion_Math& the_quat) const
{
	Quaternion_Math the_new_quat = *this;
	the_new_quat *= the_quat;
	//check_validity(new_a, new_b, new_c, new_d);
	return Quaternion_Math(the_new_quat);
}

Quaternion_Math Quaternion_Math::operator*=(const Quaternion_Math& the_quat)
{
	*this = *this * the_quat;
	return *this;
}

Quaternion_Math Quaternion_Math::normalized() const
{
	const float epsilon = 1e-6f;
	float norma = static_cast< float >(*this);

	if (norma <= epsilon)
	{
		return Quaternion_Math(0.0f, 0.0f, 0.0f, 0.0f);
	}
	return Quaternion_Math(a / norma, b / norma, c / norma, d / norma);
}

Quaternion_Math Quaternion_Math::operator*(const vector3_t& the_vector) const
{
	Quaternion_Math the_normalized_quat = (*this).normalized();
	Quaternion_Math the_vector_quat(0, the_vector.x, the_vector.y, the_vector.z);
	Quaternion_Math the_conjugate_quat = ~(the_normalized_quat);
	return (the_normalized_quat * the_vector_quat * the_conjugate_quat);
}

Quaternion_Math Quaternion_Math::operator~() const
{
	return Quaternion_Math(a, -b, -c, -d);
}

bool Quaternion_Math::operator==(const Quaternion_Math& the_quat) const
{
	const float epsilon = 1e-6f;
	return fabs(a - the_quat.a) < epsilon && fabs(b - the_quat.b) < epsilon && fabs(c - the_quat.c) < epsilon &&
		   fabs(d - the_quat.d) < epsilon;
}

bool Quaternion_Math::operator!=(const Quaternion_Math& the_quat) const
{
	return !(*this == the_quat);
}

Quaternion_Math::operator float() const
{
	return sqrt(a * a + b * b + c * c + d * d);
}

float* Quaternion_Math::data() const
{
	float* data = new float[4];
	data[0] = b;
	data[1] = c;
	data[2] = d;
	data[3] = a;
	return data;
}

float** Quaternion_Math::to_matrix() const
{
	float** the_matrix = new float*[4];

	for (int i = 0; i < 4; i++)
	{
		the_matrix[i] = new float[4];
	}

	the_matrix[0][0] = a;
	the_matrix[0][1] = b;
	the_matrix[0][2] = c;
	the_matrix[0][3] = d;
	the_matrix[1][0] = -b;
	the_matrix[1][1] = a;
	the_matrix[1][2] = d;
	the_matrix[1][3] = -c;
	the_matrix[2][0] = -c;
	the_matrix[2][1] = -d;
	the_matrix[2][2] = a;
	the_matrix[2][3] = b;
	the_matrix[3][0] = -d;
	the_matrix[3][1] = c;
	the_matrix[3][2] = -b;
	the_matrix[3][3] = a;

	return the_matrix;
}

float** Quaternion_Math::the_rotation_matrix() const
{
	float** the_rotation_matrix = new float*[3];
	for (int i = 0; i < 3; i++)
	{
		the_rotation_matrix[i] = new float[3];
	}

	the_rotation_matrix[0][0] = 1 - 2 * c * c - 2 * d * d;
	the_rotation_matrix[0][1] = 2 * b * c - 2 * a * d;
	the_rotation_matrix[0][2] = 2 * b * d + 2 * a * c;

	the_rotation_matrix[1][0] = 2 * b * c + 2 * a * d;
	the_rotation_matrix[1][1] = 1 - 2 * b * b - 2 * d * d;
	the_rotation_matrix[1][2] = 2 * c * d - 2 * a * b;

	the_rotation_matrix[2][0] = 2 * b * d - 2 * a * c;
	the_rotation_matrix[2][1] = 2 * c * d + 2 * a * b;
	the_rotation_matrix[2][2] = 1 - 2 * b * b - 2 * c * c;

	return the_rotation_matrix;
}

void Quaternion_Math::free_matrix(float** the_matrix, int the_rows)
{
	if (the_matrix)
	{
		for (int i = 0 ; i < the_rows; i++)
		{
			delete[] the_matrix[i];
		}
	}
	delete[] the_matrix;
}


