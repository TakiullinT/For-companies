// #pragma once
#ifndef QUAT_H
#define QUAT_H

#define PI 3.14159265358979323846f
#include <iostream>

struct vector3_t
{
	float x, y, z;
};

class Quaternion_Math
{
  private:
	float a, b, c, d;

  public:
	float get_a() const { return a; }
	float get_b() const { return b; }
	float get_c() const { return c; }
	float get_d() const { return d; }
	Quaternion_Math();
	Quaternion_Math(float a, float b, float c, float d);
	Quaternion_Math(float angle_radians, bool is_degrees, const vector3_t& rotation_axis);

	Quaternion_Math operator+(const Quaternion_Math& the_quat) const;
	Quaternion_Math operator-(const Quaternion_Math& the_quat) const;
	Quaternion_Math operator+=(const Quaternion_Math& the_quat);
	Quaternion_Math operator-=(const Quaternion_Math& the_quat);
	Quaternion_Math operator*(const Quaternion_Math& the_quat) const;
	Quaternion_Math operator*(const vector3_t& the_vector) const;
	Quaternion_Math operator*(float scalar) const;
	Quaternion_Math operator*=(float scalar);
	Quaternion_Math operator*=(const Quaternion_Math& the_quat);
	Quaternion_Math operator~() const;
	Quaternion_Math normalized() const;

	bool operator==(const Quaternion_Math& the_quat) const;
	bool operator!=(const Quaternion_Math& the_quat) const;

	explicit operator float() const;


	float* data() const;
	float** to_matrix() const;
	float** the_rotation_matrix() const;

	static void free_matrix(float** the_matrix, int the_rows);
	

};

#endif
