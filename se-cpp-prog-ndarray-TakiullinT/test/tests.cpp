#include "initializer_parser.hpp"
#include "ndarray.hpp"
#include "ndarray_iterator.hpp"
#include "ndarray_view.hpp"

#include <gtest/gtest.h>

// ТЕСТЫ ITERATOR
TEST(NDArrayIteratorTest, ConstructionAndDereference)
{
	int value = 42;
	NDArrayIterator< int > it(&value);

	EXPECT_EQ(*it, 42);
	EXPECT_EQ(*it, value);

	EXPECT_EQ(it.operator->(), &value);
}

TEST(NDArrayIteratorTest, IteratorOnEmptyArray)
{
	int *data = nullptr;
	NDArrayView< int, 1 > view(data, { 0 }, { 1 });
	auto begin = view.begin();
	auto end = view.end();
	EXPECT_EQ(begin, end);
}

TEST(NDArrayIteratorTest, IncrementDecrement)
{
	int arr[3] = { 1, 2, 3 };
	NDArrayIterator< int > it(arr);

	EXPECT_EQ(*++it, 2);

	EXPECT_EQ(*it++, 2);
	EXPECT_EQ(*it, 3);

	EXPECT_EQ(*--it, 2);

	EXPECT_EQ(*it--, 2);
	EXPECT_EQ(*it, 1);
}

TEST(NDArrayIteratorTest, ArithmeticOperations)
{
	int arr[5] = { 1, 2, 3, 4, 5 };
	NDArrayIterator< int > it1(arr);
	NDArrayIterator< int > it2(arr + 3);

	EXPECT_EQ(*(it1 + 2), 3);

	EXPECT_EQ(it2 - it1, 3);
	EXPECT_EQ(*(it2 - 1), 3);

	it1 += 4;
	EXPECT_EQ(*it1, 5);
	it1 -= 2;
	EXPECT_EQ(*it1, 3);
}

TEST(NDArrayIteratorTest, ComparisonOperators)
{
	int arr[3] = { 1, 2, 3 };
	NDArrayIterator< int > it1(arr);
	NDArrayIterator< int > it2(arr + 1);
	NDArrayIterator< int > it3(arr + 1);

	EXPECT_TRUE(it1 < it2);
	EXPECT_TRUE(it1 <= it2);
	EXPECT_TRUE(it2 > it1);
	EXPECT_TRUE(it2 >= it1);
	EXPECT_TRUE(it2 == it3);
	EXPECT_TRUE(it1 != it2);
}

TEST(NDArrayIteratorTest, RandomAccess)
{
	int arr[5] = { 10, 20, 30, 40, 50 };
	NDArrayIterator< int > it(arr);

	EXPECT_EQ(it[0], 10);
	EXPECT_EQ(it[2], 30);
	EXPECT_EQ(it[4], 50);

	EXPECT_EQ(*(it + 3), 40);
	EXPECT_EQ(*(it += 2), 30);
	EXPECT_EQ(it[-1], 20);
}

TEST(NDArrayIteratorTest, ConstCorrectness)
{
	int value = 100;
	const NDArrayIterator< int > const_it(&value);
	NDArrayIterator< int > mutable_it(&value);

	EXPECT_EQ(*const_it, 100);
	EXPECT_EQ(*mutable_it, 100);

	*mutable_it = 200;
	EXPECT_EQ(*const_it, 200);
}

TEST(NDArrayIteratorTest, CopyAssignmentAndConstruction)
{
	int arr[] = { 1, 2, 3 };
	NDArrayIterator< int > it1(arr);
	NDArrayIterator< int > it2 = it1;
	NDArrayIterator< int > it3(arr + 1);
	it3 = it1;

	EXPECT_EQ(*it2, 1);
	EXPECT_EQ(*it3, 1);
}

// ТЕСТЫ VIEW
TEST(NDArrayViewTest, DefaultConstructor)
{
	NDArrayView< int, 2 > view;
	EXPECT_EQ(view.data(), nullptr);
	EXPECT_TRUE(view.empty());
}

TEST(NDArrayViewTest, ConstructFromDataAndShape)
{
	int data[] = { 1, 2, 3, 4, 5, 6 };
	std::array< std::size_t, 2 > sizes = { 2, 3 };
	std::array< std::size_t, 2 > strides = { 3, 1 };
	NDArrayView< int, 2 > view(data, sizes, strides);

	EXPECT_EQ(view.count(), 2);
	EXPECT_EQ(view.total_count(), 6);
	EXPECT_EQ(view[0][0], 1);
	EXPECT_EQ(view[1][2], 6);
}

TEST(NDArrayViewTest, CopyAndMoveConstructors)
{
	int data[] = { 10, 20 };
	std::array< std::size_t, 1 > sizes = { 2 };
	std::array< std::size_t, 1 > strides = { 1 };
	NDArrayView< int, 1 > original(data, sizes, strides);
	NDArrayView< int, 1 > copy = original;
	NDArrayView< int, 1 > moved = std::move(original);

	EXPECT_EQ(copy[1], 20);
	EXPECT_EQ(moved[0], 10);
}

TEST(NDArrayTest, MoveConstructors)
{
	NDArray< int, 1 > original{ 1, 2, 3, 4, 5 };
	NDArray< int, 1 > moved(std::move(original));
	EXPECT_EQ(*moved[0].data(), 1);
	EXPECT_EQ(*moved[1].data(), 2);
	EXPECT_TRUE(original.data() == nullptr);
}

TEST(NDArrayViewTest, IteratorSupport)
{
	int data[] = { 1, 2, 3, 4 };
	NDArrayView< int, 1 > view(data, { 4 }, { 1 });
	int sum = 0;
	for (auto it = view.begin(); it != view.end(); it++)
	{
		sum += *it;
	}
	EXPECT_EQ(sum, 10);
}

TEST(NDArrayViewTest, RangeConstructorWithValidRange)
{
	std::vector< int > vec = { 1, 2, 3, 4 };
	NDArrayView< int, 1 > view(vec.begin(), vec.end(), { 4 }, { 1 });
	EXPECT_EQ(view.total_count(), 4);
	EXPECT_EQ(view[2], 3);
}

TEST(NDArrayViewTest, DISABOutOfRangeIndexThrows)
{
	int data[] = { 1, 2, 3 };
	NDArrayView< int, 1 > view(data, { 3 }, { 1 });
	EXPECT_NO_THROW(view[3]);
}

TEST(NDArrayConstViewTest, NestedAccessAndConstCorrectness)
{
	int data[] = { 1, 2, 3, 4, 5, 6 };
	NDArrayConstView< int, 2 > view(data, { 2, 3 }, { 3, 1 });
	EXPECT_EQ(view[0][1], 2);
	EXPECT_EQ(view[1][2], 6);
}

TEST(NDArrayConstViewTest, EqualityOperators)
{
	int data[] = { 1, 2, 3, 4 };
	NDArrayView< int, 1 > view1(data, { 4 }, { 1 });
	NDArrayView< int, 1 > view2(data, { 4 }, { 1 });
	NDArrayView< int, 1 > view3(data, { 2 }, { 1 });

	EXPECT_TRUE(view1 == view2);
	EXPECT_FALSE(view1 != view2);
	EXPECT_FALSE(view1 == view3);
}

TEST(NDArrayViewTest, SlicingReturnsCorrectSubView)
{
	int data[] = { 1, 2, 3, 4, 5, 6 };
	NDArrayView< int, 2 > view(data, { 2, 3 }, { 3, 1 });
	auto row1 = view[1];
	EXPECT_EQ(row1[0], 4);
	EXPECT_EQ(row1[2], 6);
}

// ТЕСТЫ NDARRAY
TEST(NDArrayTest, ConstructorWithSizes)
{
	NDArray< int, 2 > arr({ 2, 3 });
	EXPECT_EQ(arr.total_count(), 6);
	EXPECT_EQ(arr.dim(), 2);
}

TEST(NDArrayTest, ConstructorWithValue)
{
	NDArray< int, 2 > arr({ 2, 2 }, 42);
	for (size_t i = 0; i < arr.count(); i++)
	{
		for (size_t j = 0; j < arr[i].count(); j++)
		{
			EXPECT_EQ(arr[i][j], 42);
		}
	}
}

TEST(NDArrayTest, CopyConstructor)
{
	NDArray< int, 2 > arr1({ 2, 2 }, 5);
	NDArray< int, 2 > arr2(arr1);
	EXPECT_TRUE(arr1.is_equal(arr2));
}

TEST(NDArrayTest, MoveConstructor)
{
	NDArray< int, 2 > arr1({ 2, 2 }, 9);
	NDArray< int, 2 > arr2(std::move(arr1));
	EXPECT_EQ(arr2.total_count(), 4);
}

TEST(NDArrayTest, CopyAssignment)
{
	NDArray< int, 2 > arr1({ 2, 2 }, 3);
	NDArray< int, 2 > arr2;
	arr2 = arr1;
	EXPECT_TRUE(arr2.is_equal(arr1));
}

TEST(NDArrayTest, MoveAssignment)
{
	NDArray< int, 2 > arr1({ 2, 2 }, 3);
	NDArray< int, 2 > arr2;
	arr2 = std::move(arr1);
	EXPECT_EQ(arr2.total_count(), 4);
}

TEST(NDArrayTest, InitializerListConstructor)
{
	NDArray< int, 2 > arr{ { { 1, 2 }, { 3, 4 } } };
	EXPECT_EQ(arr[0][0], 1);
	EXPECT_EQ(arr[0][1], 2);
	EXPECT_EQ(arr[1][0], 3);
	EXPECT_EQ(arr[1][1], 4);
}

TEST(NDArrayTest, ReshapeValid)
{
	NDArray< int, 2 > arr({ 2, 2 }, 1);
	auto reshaped = arr.reshape(std::array< std::size_t, 2 >{ 1, 4 });
	EXPECT_EQ(reshaped.total_count(), 4);
}

TEST(NDArrayTest, ReshapeInvalid)
{
	NDArray< int, 2 > arr({ 2, 2 }, 1);
	EXPECT_THROW(arr.reshape(std::array< std::size_t, 2 >{ 3, 3 }), std::invalid_argument);
}

TEST(NDArrayTest, DISABLED_OperatorSquareBracketsDoesNotThrowOnOutOfRange)
{
	NDArray< int, 1 > arr({ 5 });
	EXPECT_NO_THROW(arr[5]);
	EXPECT_NO_THROW(arr[-1]);
	EXPECT_NO_THROW(arr[10]);
}

TEST(NDArrayTest, Reshape)
{
	NDArray< int, 2 > arr({ 2, 6 }, 1);
	for (int i = 0; i < 12; i++)
	{
		arr.data()[i] = i + 1;
	}

	EXPECT_EQ(arr.total_count(), 12);
	EXPECT_EQ(arr[0][0], 1);
	EXPECT_EQ(arr[0][1], 2);
	EXPECT_EQ(arr[1][4], 11);
	EXPECT_EQ(arr[1][5], 12);

	auto reshaped = arr.reshape(std::array< std::size_t, 3 >{ 2, 2, 3 });
	EXPECT_EQ(reshaped.total_count(), 12);
	EXPECT_EQ(reshaped[0][0][0], 1);
	EXPECT_EQ(reshaped[0][0][1], 2);
	EXPECT_EQ(reshaped[1][1][1], 11);
	EXPECT_EQ(reshaped[1][1][2], 12);

	EXPECT_NO_THROW(arr.reshape(std::array< std::size_t, 3 >{ 2, 2, 3 }));
}

TEST(NDArrayTest, SwapValid)
{
	NDArray< int, 2 > arr1({ 2, 2 }, 5);
	NDArray< int, 2 > arr2({ 2, 2 }, 8);
	arr1.swap(arr2);
	for (std::size_t i = 0; i < arr1.count(); i++)
	{
		for (std::size_t j = 0; j < arr1[i].count(); j++)
		{
			EXPECT_EQ(arr1[i][j], 8);
			EXPECT_EQ(arr2[i][j], 5);
		}
	}
}

TEST(NDArrayTest, SwapDataCheck)
{
	NDArray< int, 2 > arr1({ 2, 2 }, 1);
	NDArray< int, 2 > arr2({ 2, 2 }, 2);

	arr1.swap(arr2);

	for (size_t i = 0; i < 2; i++)
	{
		for (size_t j = 0; j < 2; j++)
		{
			EXPECT_EQ(arr1[i][j], 2);
			EXPECT_EQ(arr2[i][j], 1);
		}
	}
}

TEST(NDArray, SwapIncludeCheck)
{
	NDArray< int, 2 > arr1({ 2, 2 }, 1);
	NDArray< int, 2 > arr2({ 2, 2 }, 2);

	std::swap(arr1, arr2);

	for (size_t i = 0; i < 2; i++)
	{
		for (size_t j = 0; j < 2; j++)
		{
			EXPECT_EQ(arr1[i][j], 2);
			EXPECT_EQ(arr2[i][j], 1);
		}
	}
}

TEST(NDArrayTest, SwapInvalidShape)
{
	NDArray< int, 2 > arr1({ 2, 2 }, 5);
	NDArray< int, 2 > arr2({ 3, 2 }, 8);
	EXPECT_THROW(arr1.swap(arr2), std::invalid_argument);
}

TEST(NDArrayTest, SwapSameData)
{
	NDArray< int, 2 > arr1({ 2, 2 }, 5);
	EXPECT_NO_THROW(arr1.swap(arr1));
}

TEST(NDArrayTest, AtAccessReturnsCorrectValues)
{
	NDArray< int, 2 > m{ { 1, 2 }, { 3, 4 } };

	EXPECT_EQ(m.at({ 0, 0 })[0], 1);
	EXPECT_EQ(m.at({ 0, 1 })[0], 2);
	EXPECT_EQ(m.at({ 1, 0 })[0], 3);
	EXPECT_EQ(m.at({ 1, 1 })[0], 4);
}

TEST(NDArrayTest, DISABLED_AtThrowsOutOfBounds)
{
	NDArray< int, 2 > m{ { 1, 2 }, { 3, 4 } };

	EXPECT_THROW(m.at({ 2, 0 }), std::out_of_range);
	EXPECT_THROW(m.at({ 0, 2 }), std::out_of_range);
	EXPECT_THROW(m.at({}), std::invalid_argument);
	EXPECT_THROW(m.at({ 0 }), std::invalid_argument);
}

TEST(NDArrayTest, AtWithEmptyIndexThrows)
{
	NDArray< int, 2 > arr({ 2, 2 }, 5);
	EXPECT_THROW(arr.at({}), std::invalid_argument);
	EXPECT_THROW(arr.at({}), std::invalid_argument);
}

int main(int argc, char **argv)
{
	testing::InitGoogleTest(&argc, argv);
	return RUN_ALL_TESTS();
}
