#include "quat.h"

#include <gtest/gtest.h>

#include <cmath>
#include <stdexcept>

TEST(Quaternion_Constructors, Default_Constructor)
{
	Quaternion_Math the_quat;
	EXPECT_EQ(the_quat.get_a(), 0);
	EXPECT_EQ(the_quat.get_b(), 0);
	EXPECT_EQ(the_quat.get_c(), 0);
	EXPECT_EQ(the_quat.get_d(), 0);
}

TEST(Quaternion_Constructors, Parameterized_Constructor)
{
	Quaternion_Math the_quat(1, 2, 3, 4);
	EXPECT_EQ(the_quat.get_a(), 1);
	EXPECT_EQ(the_quat.get_b(), 2);
	EXPECT_EQ(the_quat.get_c(), 3);
	EXPECT_EQ(the_quat.get_d(), 4);
}

TEST(Quaternion_Constructors, Angle_Axis_Constructor_Custom_Axis)
{
	vector3_t rotation_axis = { 1.0f, 2.0f, 3.0f };
	float angle_radians = 45.0f;

	float norm = sqrt(1.0f * 1.0f + 2.0f * 2.0f + 3.0f * 3.0f);
	float half_angle_radians = (angle_radians * PI / 180.0f) / 2.0f;

	Quaternion_Math the_quat(angle_radians, true, rotation_axis);
	EXPECT_FLOAT_EQ(the_quat.get_a(), cos(half_angle_radians));
	EXPECT_FLOAT_EQ(the_quat.get_b(), (rotation_axis.x / norm) * sin(half_angle_radians));
	EXPECT_FLOAT_EQ(the_quat.get_c(), (rotation_axis.y / norm) * sin(half_angle_radians));
	EXPECT_FLOAT_EQ(the_quat.get_d(), (rotation_axis.z / norm) * sin(half_angle_radians));

	vector3_t rotation_axis_2 = { 0.0f, 0.0f, 0.0f };
	Quaternion_Math the_quat_2(angle_radians, true, rotation_axis_2);
	EXPECT_FLOAT_EQ(the_quat_2.get_a(), 1.0f);
	EXPECT_FLOAT_EQ(the_quat_2.get_b(), 0.0f);
	EXPECT_FLOAT_EQ(the_quat_2.get_c(), 0.0f);
	EXPECT_FLOAT_EQ(the_quat_2.get_d(), 0.0f);

	float negative_angle_radians = -45.0f;
	Quaternion_Math the_quat_neg(negative_angle_radians, true, rotation_axis);
	EXPECT_FLOAT_EQ(the_quat_neg.get_a(), cos(half_angle_radians));
	EXPECT_FLOAT_EQ(the_quat_neg.get_b(), -(rotation_axis.x / norm) * sin(half_angle_radians));
	EXPECT_FLOAT_EQ(the_quat_neg.get_c(), -(rotation_axis.y / norm) * sin(half_angle_radians));
	EXPECT_FLOAT_EQ(the_quat_neg.get_d(), -(rotation_axis.z / norm) * sin(half_angle_radians));

	vector3_t rotation_axis_3 = { 1.0f, 0.0f, 0.0f };
	Quaternion_Math the_quat_3(PI / 2.0f, false, rotation_axis_3);
	float half_angle_radians_2 = (PI / 2.0f) / 2.0f;
	EXPECT_NEAR(the_quat_3.get_a(), cos(half_angle_radians_2), 1e-6f);
	EXPECT_NEAR(the_quat_3.get_b(), sin(half_angle_radians_2), 1e-6f);
	EXPECT_NEAR(the_quat_3.get_c(), 0.0f, 1e-6f);
	EXPECT_NEAR(the_quat_3.get_d(), 0.0f, 1e-6f);
}

TEST(Quaternion_Constructors, Angle_Axis_Constructor)
{
	vector3_t rotation_axis = { 1, 0, 0 };
	Quaternion_Math the_quat(180, true, rotation_axis);
	EXPECT_NEAR(the_quat.get_a(), 0, 1e-6f);
	EXPECT_NEAR(the_quat.get_b(), 1, 1e-6f);
	EXPECT_NEAR(the_quat.get_c(), 0, 1e-6f);
	EXPECT_NEAR(the_quat.get_d(), 0, 1e-6f);
}

TEST(Quaternion_Constructors, Zero_Axis_With_Different_Angles)
{
	vector3_t rotation_axis = { 0, 0, 0 };
	Quaternion_Math the_quat_1(45.0f, true, rotation_axis);
	Quaternion_Math the_quat_2(180.0f, false, rotation_axis);
	Quaternion_Math expected(1.0f, 0.0f, 0.0f, 0.0f);

	EXPECT_FLOAT_EQ(the_quat_1.get_a(), expected.get_a());
	EXPECT_FLOAT_EQ(the_quat_1.get_b(), expected.get_b());
	EXPECT_FLOAT_EQ(the_quat_1.get_c(), expected.get_c());
	EXPECT_FLOAT_EQ(the_quat_1.get_d(), expected.get_d());
	EXPECT_EQ(the_quat_1, the_quat_2);
}

TEST(Quaternion_Arithmetic, Addition)
{
	Quaternion_Math the_quat_1(1, 2, 3, 4);
	Quaternion_Math the_quat_2(5, 6, 7, 8);
	Quaternion_Math the_quat_3 = the_quat_1 + the_quat_2;
	EXPECT_EQ(the_quat_3.get_a(), 6);
	EXPECT_EQ(the_quat_3.get_b(), 8);
	EXPECT_EQ(the_quat_3.get_c(), 10);
	EXPECT_EQ(the_quat_3.get_d(), 12);
}

TEST(Quaternion_Arithmetic, Subtraction)
{
	Quaternion_Math the_quat_1(1, 2, 3, 4);
	Quaternion_Math the_quat_2(5, 6, 7, 8);
	Quaternion_Math the_quat_3 = the_quat_1 - the_quat_2;
	EXPECT_EQ(the_quat_3.get_a(), -4);
	EXPECT_EQ(the_quat_3.get_b(), -4);
	EXPECT_EQ(the_quat_3.get_c(), -4);
	EXPECT_EQ(the_quat_3.get_d(), -4);
}

TEST(Quaternion_Arithmetic, Addition_Commutativity)
{
	Quaternion_Math the_quat_1(1, 2, 3, 4);
	Quaternion_Math the_quat_2(5, 6, 7, 8);
	Quaternion_Math the_sum_1 = the_quat_1 + the_quat_2;
	Quaternion_Math the_sum_2 = the_quat_2 + the_quat_1;
	EXPECT_EQ(the_sum_1, the_sum_2);
}

TEST(Quaternion_Arithmetic, Addition_Associativity)
{
	Quaternion_Math the_quat_1(1, 2, 3, 4);
	Quaternion_Math the_quat_2(5, 6, 7, 8);
	Quaternion_Math the_quat_3(9, 10, 11, 12);
	Quaternion_Math the_sum_1 = (the_quat_1 + the_quat_2) + the_quat_3;
	Quaternion_Math the_sum_2 = the_quat_1 + (the_quat_2 + the_quat_3);
	EXPECT_EQ(the_sum_1, the_sum_2);
}

TEST(Quaternion_Arithmetic, Subtraction_NonCommutativity)
{
	Quaternion_Math the_quat_1(1, 2, 3, 4);
	Quaternion_Math the_quat_2(5, 6, 7, 8);
	Quaternion_Math the_diff_1 = the_quat_1 - the_quat_2;
	Quaternion_Math the_diff_2 = the_quat_2 - the_quat_1;
	EXPECT_NE(the_diff_1, the_diff_2);
	Quaternion_Math zero(0, 0, 0, 0);
	EXPECT_EQ(the_diff_1 + the_diff_2, zero);
}

TEST(Quaternion_Arithmetic, Subtraction_NonAssociativity)
{
	Quaternion_Math the_quat_1(10, 20, 30, 40);
	Quaternion_Math the_quat_2(1, 2, 3, 4);
	Quaternion_Math the_quat_3(5, 6, 7, 8);
	Quaternion_Math the_result_1 = (the_quat_1 - the_quat_2) - the_quat_3;
	Quaternion_Math the_result_2 = the_quat_1 - (the_quat_2 - the_quat_3);
	EXPECT_NE(the_result_1, the_result_2);
}

TEST(Quaternion_Arithmetic, Multiplication)
{
	Quaternion_Math the_quat_1(1, 2, 3, 4);
	Quaternion_Math the_quat_2(5, 6, 7, 8);
	Quaternion_Math the_quat_3 = the_quat_1 * the_quat_2;
	EXPECT_EQ(the_quat_3.get_a(), -60);
	EXPECT_EQ(the_quat_3.get_b(), 12);
	EXPECT_EQ(the_quat_3.get_c(), 30);
	EXPECT_EQ(the_quat_3.get_d(), 24);
}

TEST(Quaternion_Arithmetic, Non_Commutative_Multiplication)
{
	Quaternion_Math the_quat_1(1, 2, 3, 4);
	Quaternion_Math the_quat_2(5, 6, 7, 8);
	Quaternion_Math composition_1 = the_quat_1 * the_quat_2;
	Quaternion_Math composition_2 = the_quat_2 * the_quat_1;
	EXPECT_NE(composition_1, composition_2);
}

TEST(Quaternion_Arithmetic, Negative_Scalar_Multiplication)
{
	Quaternion_Math the_quat(1, -2, 3, -4);
	Quaternion_Math the_result = the_quat * (-2);
	EXPECT_FLOAT_EQ(the_result.get_a(), -2);
	EXPECT_FLOAT_EQ(the_result.get_b(), 4);
	EXPECT_FLOAT_EQ(the_result.get_c(), -6);
	EXPECT_FLOAT_EQ(the_result.get_d(), 8);
}

TEST(Quaternion_Arithmetic, Multiply_By_Zero_Quaternion)
{
	Quaternion_Math the_quat_1(1, 2, 3, 4);
	Quaternion_Math the_zero_quat(0, 0, 0, 0);
	Quaternion_Math the_result = the_quat_1 * the_zero_quat;
	EXPECT_EQ(the_result.get_a(), 0);
	EXPECT_EQ(the_result.get_b(), 0);
	EXPECT_EQ(the_result.get_c(), 0);
	EXPECT_EQ(the_result.get_d(), 0);
}

TEST(Quaternion_Arithmetic, Scalar_Multiplication)
{
	Quaternion_Math the_quat_1(1, 2, 3, 4);
	Quaternion_Math the_quat_2 = the_quat_1 * 2;
	EXPECT_EQ(the_quat_2.get_a(), 2);
	EXPECT_EQ(the_quat_2.get_b(), 4);
	EXPECT_EQ(the_quat_2.get_c(), 6);
	EXPECT_EQ(the_quat_2.get_d(), 8);

	float max_val = 3.402823466e+38f;
	Quaternion_Math q(max_val, max_val, max_val, max_val);

	EXPECT_THROW({ q * 2.0f; }, std::overflow_error);
}

TEST(Quaternion_Arithmetic, Scalar_Multiplication_Assignment)
{
	Quaternion_Math the_quat_1(1, 2, 3, 4);
	the_quat_1 *= 2;
	EXPECT_EQ(the_quat_1.get_a(), 2);
	EXPECT_EQ(the_quat_1.get_b(), 4);
	EXPECT_EQ(the_quat_1.get_c(), 6);
	EXPECT_EQ(the_quat_1.get_d(), 8);
}

TEST(Quaternion_Arithmetic, Quaternion_Multiplication_Assignment)
{
	Quaternion_Math the_quat_1(1, 2, 3, 4);
	Quaternion_Math the_quat_2(5, 6, 7, 8);
	the_quat_1 *= the_quat_2;
	EXPECT_EQ(the_quat_1.get_a(), -60);
	EXPECT_EQ(the_quat_1.get_b(), 12);
	EXPECT_EQ(the_quat_1.get_c(), 30);
	EXPECT_EQ(the_quat_1.get_d(), 24);
}

TEST(Quaternion_Arithmetic, Self_Assignment)
{
	Quaternion_Math the_quat(1, 2, 3, 4);
	Quaternion_Math copy = the_quat;
	the_quat *= the_quat;
	Quaternion_Math expected = copy * copy;
	EXPECT_FLOAT_EQ(the_quat.get_a(), expected.get_a());
	EXPECT_FLOAT_EQ(the_quat.get_b(), expected.get_b());
	EXPECT_FLOAT_EQ(the_quat.get_c(), expected.get_c());
	EXPECT_FLOAT_EQ(the_quat.get_d(), expected.get_d());
}

TEST(Quaternion_Arithmetic, Associativity)
{
	Quaternion_Math the_quat_1(1, 2, 3, 4);
	Quaternion_Math the_quat_2(5, 6, 7, 8);
	Quaternion_Math the_quat_3(9, 10, 11, 12);
	Quaternion_Math the_result_1 = (the_quat_1 * the_quat_2) * the_quat_3;
	Quaternion_Math the_result_2 = the_quat_1 * (the_quat_2 * the_quat_3);
	EXPECT_TRUE(the_result_1 == the_result_2);
}

TEST(Quaternion_Properties, Conjugate)
{
	Quaternion_Math the_quat(1, 2, 3, 4);
	Quaternion_Math the_conjugate_quat = ~the_quat;
	EXPECT_EQ(the_conjugate_quat.get_a(), 1);
	EXPECT_EQ(the_conjugate_quat.get_b(), -2);
	EXPECT_EQ(the_conjugate_quat.get_c(), -3);
	EXPECT_EQ(the_conjugate_quat.get_d(), -4);
}

TEST(Quaternion_Properties, Equality_And_Inequality)
{
	Quaternion_Math the_quat_1(1.5, 2.09, 3.0, 4.005);
	Quaternion_Math the_quat_2(1.5, 2.09, 3.0, 4.005);
	Quaternion_Math the_quat_3(1, 2, 3, 4);
	EXPECT_EQ(the_quat_1, the_quat_2);
	EXPECT_NE(the_quat_1, the_quat_3);
}

TEST(Quaternion_Properties, Epsilon_Equality)
{
	Quaternion_Math the_quat_1(1.0000001f, 0, 0, 0);
	Quaternion_Math the_quat_2(1.0000002f, 0, 0, 0);
	EXPECT_TRUE(the_quat_1 == the_quat_2);
}

TEST(Quaternion_Properties, Quaternion_To_Float_Conversion)
{
	Quaternion_Math the_quat_1(3.0f, 4.0f, 0.0f, 0.0f);
	Quaternion_Math the_quat_2(0.0f, 3.0f, 0.0f, 0.0f);
	Quaternion_Math the_quat_3(0.0f, 0.0f, 0.0f, 0.0f);

	EXPECT_FLOAT_EQ(static_cast< float >(the_quat_1), 5.0f);
	EXPECT_FLOAT_EQ(static_cast< float >(the_quat_2), 3.0f);
	EXPECT_FLOAT_EQ(static_cast< float >(the_quat_3), 0.0f);
}

TEST(Quaternion_DataConversion, Data)
{
	Quaternion_Math the_quat(1, 2, 3, 4);
	float* data = the_quat.data();
	EXPECT_EQ(data[0], 2);
	EXPECT_EQ(data[1], 3);
	EXPECT_EQ(data[2], 4);
	EXPECT_EQ(data[3], 1);
	delete[] data;
}

TEST(Quaternion_DataConversion, To_Matrix)
{
	Quaternion_Math the_quat(2, 3, 4, 1);
	float** the_matrix = the_quat.to_matrix();
	EXPECT_EQ(the_matrix[0][0], 2);
	EXPECT_EQ(the_matrix[0][1], 3);
	EXPECT_EQ(the_matrix[0][2], 4);
	EXPECT_EQ(the_matrix[0][3], 1);
	EXPECT_EQ(the_matrix[1][0], -3);
	EXPECT_EQ(the_matrix[1][1], 2);
	EXPECT_EQ(the_matrix[1][2], 1);
	EXPECT_EQ(the_matrix[1][3], -4);
	EXPECT_EQ(the_matrix[2][0], -4);
	EXPECT_EQ(the_matrix[2][1], -1);
	EXPECT_EQ(the_matrix[2][2], 2);
	EXPECT_EQ(the_matrix[2][3], 3);
	EXPECT_EQ(the_matrix[3][0], -1);
	EXPECT_EQ(the_matrix[3][1], 4);
	EXPECT_EQ(the_matrix[3][2], -3);
	EXPECT_EQ(the_matrix[3][3], 2);


	Quaternion_Math::free_matrix(the_matrix, 4);
}

TEST(Quaternion_DataConversion, the_rotation_matrix)
{
	Quaternion_Math the_quat(2, 3, 4, 1);
	float** the_rotation_matrix = the_quat.the_rotation_matrix();
	EXPECT_EQ(the_rotation_matrix[0][0], -33);
	EXPECT_EQ(the_rotation_matrix[0][1], 20);
	EXPECT_EQ(the_rotation_matrix[0][2], 22);
	EXPECT_EQ(the_rotation_matrix[1][0], 28);
	EXPECT_EQ(the_rotation_matrix[1][1], -19);
	EXPECT_EQ(the_rotation_matrix[1][2], -4);
	EXPECT_EQ(the_rotation_matrix[2][0], -10);
	EXPECT_EQ(the_rotation_matrix[2][1], 20);
	EXPECT_EQ(the_rotation_matrix[2][2], -49);

	Quaternion_Math::free_matrix(the_rotation_matrix, 3);
	
}

TEST(Quaternion_DataConversion, Identity_Multiplication)
{
	Quaternion_Math the_identity_quat(1, 0, 0, 0);
	Quaternion_Math the_quat(2, 3, 4, 5);
	Quaternion_Math the_composition = the_quat * the_identity_quat;
	EXPECT_FLOAT_EQ(the_composition.get_a(), the_quat.get_a());
	EXPECT_FLOAT_EQ(the_composition.get_b(), the_quat.get_b());
	EXPECT_FLOAT_EQ(the_composition.get_c(), the_quat.get_c());
	EXPECT_FLOAT_EQ(the_composition.get_d(), the_quat.get_d());
}

TEST(Quaternion_Rotation, Vector_Rotation)
{
	Quaternion_Math the_quat(90.0f, true, { 0, 0, 1 });
	vector3_t the_vector = { 1, 0, 0 };
	Quaternion_Math the_rotated_quat = the_quat * the_vector;
	EXPECT_NEAR(the_rotated_quat.get_b(), 0.0f, 1e-6f);
	EXPECT_NEAR(the_rotated_quat.get_c(), 1.0f, 1e-6f);
	EXPECT_NEAR(the_rotated_quat.get_d(), 0.0f, 1e-6f);

	Quaternion_Math the_quat_2(90.0f, true, { 0, 0, 1 });
	the_quat_2 *= 2.0f;
	Quaternion_Math the_rotated_quat_2 = the_quat_2 * the_vector;
	EXPECT_NEAR(the_rotated_quat_2.get_b(), 0.0f, 1e-6f);
	EXPECT_NEAR(the_rotated_quat_2.get_c(), 1.0f, 1e-6f);
	EXPECT_NEAR(the_rotated_quat_2.get_d(), 0.0f, 1e-6f);
}

TEST(Quaternion_ExtremeValues, Large_Values_Operators)
{
	float big_val = 3.402823466e+38f;
	Quaternion_Math the_quat_1(big_val, big_val, big_val, big_val);
	Quaternion_Math the_quat_2(big_val, big_val, big_val, big_val);

	EXPECT_THROW({ Quaternion_Math the_addition_result = the_quat_1 + the_quat_2; }, std::overflow_error);

	Quaternion_Math the_subtraction_result = the_quat_1 - the_quat_2;
	EXPECT_FLOAT_EQ(the_subtraction_result.get_a(), 0.0f);
	EXPECT_FLOAT_EQ(the_subtraction_result.get_b(), 0.0f);
	EXPECT_FLOAT_EQ(the_subtraction_result.get_c(), 0.0f);
	EXPECT_FLOAT_EQ(the_subtraction_result.get_d(), 0.0f);

	EXPECT_THROW({ Quaternion_Math the_multiply_result = the_quat_1 * the_quat_2; }, std::overflow_error);
}

TEST(Quaternion_Constructors, Large_Values_Constructor)
{
	vector3_t large_rotation_axis = { 1000.0f, 2000.0f, 3000.0f };
	Quaternion_Math the_quat_1(720.0f, true, large_rotation_axis);

	float axis_norm = sqrt(1000.0f * 1000.0f + 2000.0f * 2000.0f + 3000.0f * 3000.0f);
	float normalized_x = 1000.0f / axis_norm;
	float normalized_y = 2000.0f / axis_norm;
	float normalized_z = 3000.0f / axis_norm;

	EXPECT_NEAR(the_quat_1.get_a(), 1.0f, 1e-6f);
	EXPECT_NEAR(the_quat_1.get_b(), 0.0f, 1e-6f);
	EXPECT_NEAR(the_quat_1.get_c(), 0.0f, 1e-6f);
	EXPECT_NEAR(the_quat_1.get_d(), 0.0f, 1e-6f);

	vector3_t huge_rotation_axis = { 1e6f, 2e6f, 3e6f };
	Quaternion_Math the_quat_2(180.0f, true, huge_rotation_axis);

	axis_norm = sqrt(1e12f + 4e12f + 9e12f);
	normalized_x = 1e6f / axis_norm;
	normalized_y = 2e6f / axis_norm;
	normalized_z = 3e6f / axis_norm;

	float half_angle_radians = (180.0f * PI / 180.0f) / 2.0f;
	EXPECT_NEAR(the_quat_2.get_a(), cos(half_angle_radians), 1e-6f);
	EXPECT_NEAR(the_quat_2.get_b(), normalized_x * sin(half_angle_radians), 1e-6f);
	EXPECT_NEAR(the_quat_2.get_c(), normalized_y * sin(half_angle_radians), 1e-6f);
	EXPECT_NEAR(the_quat_2.get_d(), normalized_z * sin(half_angle_radians), 1e-6f);

	vector3_t negative_rotation_axis = { -5000.0f, -10000.0f, -15000.0f };
	Quaternion_Math the_quat_3(45.0f, true, negative_rotation_axis);

	axis_norm = sqrt(25e6f + 100e6f + 225e6f);
	normalized_x = -5000.0f / axis_norm;
	normalized_y = -10000.0f / axis_norm;
	normalized_z = -15000.0f / axis_norm;

	half_angle_radians = (45.0f * PI / 180.0f) / 2.0f;
	EXPECT_NEAR(the_quat_3.get_a(), cos(half_angle_radians), 1e-6f);
	EXPECT_NEAR(the_quat_3.get_b(), normalized_x * sin(half_angle_radians), 1e-6f);
	EXPECT_NEAR(the_quat_3.get_c(), normalized_y * sin(half_angle_radians), 1e-6f);
	EXPECT_NEAR(the_quat_3.get_d(), normalized_z * sin(half_angle_radians), 1e-6f);
}

TEST(Quaternion_Arithmetic, Addition_Overflow)
{
	float max_val = 3.402823466e+38f;
	Quaternion_Math the_quat_1(max_val, max_val, max_val, max_val);
	Quaternion_Math the_quat_2(max_val, max_val, max_val, max_val);
	EXPECT_THROW(the_quat_1 += the_quat_2, std::overflow_error);
}

TEST(Quaternion_Arithmetic, Subtraction_Overflow)
{
	float max_val = 3.402823466e+38f;
	Quaternion_Math the_quat_1(-max_val, -max_val, -max_val, -max_val);
	Quaternion_Math the_quat_2(max_val, max_val, max_val, max_val);
	EXPECT_THROW(the_quat_1 -= the_quat_2, std::overflow_error);
}

TEST(Quaternion_Arithmetic, ScalarMultiplication_Overflow)
{
	float max_val = 3.402823466e+38f;
	Quaternion_Math the_quat(max_val, max_val, max_val, max_val);
	EXPECT_THROW(the_quat *= 2.0f, std::overflow_error);
}

TEST(Quaternion_Arithmetic, QuaternionMultiplication_Overflow)
{
	float big_val = 3.402823466e+38f;
	Quaternion_Math the_quat_1(big_val, big_val, big_val, big_val);
	Quaternion_Math the_quat_2(big_val, big_val, big_val, big_val);
	EXPECT_THROW(the_quat_1 *= the_quat_2, std::overflow_error);
}

int main(int argc, char** argv)
{
	::testing::InitGoogleTest(&argc, argv);
	return RUN_ALL_TESTS();
}
