#pragma once

#include "initializer_parser.hpp"
#include "ndarray_iterator.hpp"
#include "ndarray_view.hpp"

#include <array>
#include <iostream>
#include <stdexcept>

template< typename T, std::size_t N >
struct NestedInitializerList
{
	using type = std::initializer_list< typename NestedInitializerList< T, N - 1 >::type >;
};

template< typename T >
struct NestedInitializerList< T, 1 >
{
	using type = T;
};

template< typename T, std::size_t N >
class NDArray
{
  public:
	using size_type = std::size_t;
	using value_type = T;
	using pointer = T*;
	using const_pointer = const T*;
	using reference = T&;
	using const_reference = const T&;
	using view = NDArrayView< T, N - 1 >;
	using const_view = NDArrayConstView< T, N - 1 >;

	// ПРАВИЛО ПЯТИ
	NDArray() noexcept : data_(nullptr), total_size_(0) {}

	// ДЕСТРУКТОР
	~NDArray() { delete[] data_; }

	// КОПИРУЮЩИЙ ОПЕРАТОР ПРИСВАИВАНИЯ
	NDArray& operator=(const NDArray& other)
	{
		if (this != &other)
		{
			delete[] data_;
			sizes_ = other.sizes_;
			strides_ = other.strides_;
			total_size_ = other.total_size_;
			data_ = new T[total_size_];
			std::copy(other.data_, other.data_ + total_size_, data_);
		}
		return *this;
	}

	// ПЕРЕМЕЩАЮЩИЙ ОПЕРАТОР
	NDArray& operator=(NDArray&& other) noexcept
	{
		if (this != &other)
		{
			delete[] data_;
			data_ = other.data_;
			sizes_ = other.sizes_;
			strides_ = other.strides_;
			total_size_ = other.total_size_;
			other.data_ = nullptr;
			other.total_size_ = 0;
		}
		return *this;
	}

	// КОПИРУЮЩИЙ КОНСТРУКТОР
	NDArray(const NDArray& other) : sizes_(other.sizes_), strides_(other.strides_), total_size_(other.total_size_)
	{
		data_ = new T[total_size_];
		std::copy(other.data_, other.data_ + total_size_, data_);
	}
	// ПЕРЕМЕЩАЮЩИЙ КОНСТРУКТОР
	NDArray(NDArray&& other) noexcept :
		data_(other.data_), sizes_(other.sizes_), strides_(other.strides_), total_size_(other.total_size_)
	{
		other.data_ = nullptr;
		other.total_size_ = 0;
	}

	// КОНСТРУКТОР ПРИНИМАЮЩИЙ РАЗМЕРЫ КАЖДОЙ РАЗМЕРНОСТИ
	NDArray(const std::array< size_type, N >& sizes) : sizes_(sizes), total_size_(1)
	{
		for (size_type size : sizes)
		{
			total_size_ *= size;
		}
		data_ = new T[total_size_];
		strides_[N - 1] = 1;
		for (long long i = static_cast< long long >(N) - 2; i >= 0; i--)
		{
			strides_[i] = strides_[i + 1] * sizes[i + 1];
		}
	}

	// КОНСТРУКТОР ПРИНИМАЮЩИЙ РАЗМЕРЫ И ЭЛЕМЕНТ ИНИЦИАЛИЗАЦИИ
	NDArray(const std::array< size_type, N >& sizes, const T& intital_value) : sizes_(sizes)
	{
		total_size_ = 1;
		for (size_type size : sizes)
		{
			total_size_ *= size;
		}
		data_ = new T[total_size_];
		strides_[N - 1] = 1;
		for (long long i = static_cast< long long >(N) - 2; i >= 0; i--)
		{
			strides_[i] = strides_[i + 1] * sizes[i + 1];
		}
		std::fill(data_, data_ + total_size_, intital_value);
	}

	// КОНСТРУКТОР ОТ ДВУХ ИТЕРАТОРОВ ПОДХОДЯЩЕЙ РАЗМЕРНОСТИ
	template< typename InputIt >
	NDArray(const std::array< size_type, N >& sizes, InputIt first, InputIt last) : sizes_(sizes)
	{
		total_size_ = 1;
		for (size_type size : sizes)
		{
			total_size_ *= size;
		}
		if (static_cast< size_type >(std::distance(first, last)) != total_size_)
		{
			throw std::invalid_argument("NDArray: input iterator range does not match total size");
		}
		data_ = new T[total_size_];
		std::copy(first, last, data_);

		strides_[N - 1] = 1;
		for (long long i = static_cast< long long >(N) - 2; i >= 0; i--)
		{
			strides_[i] = strides_[i + 1] * sizes[i + 1];
		}
	}

	// КОНСТРУКТОР ОТ NDArrayView
	explicit NDArray(const NDArrayView< T, N >& view) :
		sizes_(view.sizes_), strides_(view.strides_), total_size_(view.total_count())
	{
		data_ = new T[total_size_];
		strides_[N - 1] = 1;
		for (long long i = static_cast< long long >(N) - 2; i >= 0; i--)
		{
			strides_[i] = strides_[i + 1] * sizes_[i + 1];
		}
		std::copy(view.begin(), view.end(), data_);
	}

	// КОНСТРУКТОР ОТ NDArrayConstView
	explicit NDArray(const NDArrayConstView< T, N >& view) :
		sizes_(view.sizes_), strides_(view.strides_), total_size_(view.total_count())
	{
		data_ = new T[total_size_];
		strides_[N - 1] = 1;
		for (long long i = static_cast< long long >(N) - 2; i >= 0; i--)
		{
			strides_[i] = strides_[i + 1] * sizes_[i + 1];
		}
		std::copy(view.begin(), view.end(), data_);
	}

	NDArray(std::initializer_list< typename NestedInitializerList< T, N >::type > init)
	{
		InitializerParser< T, N > parser;
		auto result = parser.parse(init);
		sizes_ = result.sizes;
		total_size_ = result.total_size;
		data_ = new T[total_size_];
		strides_[N - 1] = 1;
		for (long long i = static_cast< long long >(N) - 2; i >= 0; i--)
		{
			strides_[i] = strides_[i + 1] * sizes_[i + 1];
		}
		std::copy(result.data, result.data + total_size_, data_);
	}

	size_type count() const { return sizes_[0]; }

	size_type total_count() const { return total_size_; }

	size_type dim() const { return N; }

	view operator[](size_type index)
	{
		return view(data_ + index * strides_[0], slice_sizes< 1 >(), slice_strides< 1 >());
	}

	const_view operator[](size_type index) const
	{
		return const_view(data_ + index * strides_[0], slice_sizes< 1 >(), slice_strides< 1 >());
	}

	view at(std::initializer_list< size_type > indexes) const
	{
		if (indexes.size() != N)
		{
			throw std::invalid_argument("NDArray: incorrect number of indexes");
		}
		size_type offset = 0;
		for (size_type i = 0; i < N; i++)
		{
			if (indexes.begin()[i] >= sizes_[i])
			{
				throw std::out_of_range("NDArray: index out of range");
			}
			offset += indexes.begin()[i] * strides_[i];
		}
		return view(data_ + offset, slice_sizes< 1 >(), slice_strides< 1 >());
	}

	bool is_equal(const NDArray& rhs) const
	{
		if (sizes_ != rhs.sizes_ || total_size_ != rhs.total_size_)
		{
			return false;
		}
		for (size_type i = 0; i < total_size_; ++i)
		{
			if (data_[i] != rhs.data_[i])
			{
				return false;
			}
		}
		return true;
	}

	view begin() { return view(data_, slice_sizes< 1 >(), slice_strides< 1 >()); }

	view end() { return view(data_ + sizes_[0] * strides_[0], slice_sizes< 1 >(), slice_strides< 1 >()); }

	const_view begin() const { return const_view(data_, slice_sizes< 1 >(), slice_strides< 1 >()); }

	const_view end() const
	{
		return const_view(data_ + sizes_[0] * strides_[0], slice_sizes< 1 >(), slice_strides< 1 >());
	}

	const_view cbegin() const { return begin(); }

	const_view cend() const { return end(); }

	bool empty() const { return total_count() == 0; }
	template< std::size_t M >
	NDArrayView< T, M > reshape(const std::array< size_type, M >& new_sizes) const
	{
		size_type new_total_size = 1;
		for (size_type s : new_sizes)
			new_total_size *= s;

		if (new_total_size != total_size_)
			throw std::invalid_argument("NDArray: incompatible sizes for reshape");

		std::array< size_type, M > new_strides{};
		new_strides[M - 1] = 1;
		for (long long i = static_cast< long long >(M) - 2; i >= 0; --i)
			new_strides[i] = new_strides[i + 1] * new_sizes[i + 1];

		return NDArrayView< T, M >(data_, new_sizes, new_strides);
	}

	void swap(NDArray& other)
	{
		if (sizes_ != other.sizes_ || strides_ != other.strides_)
		{
			throw std::invalid_argument("NDArray: cannot swap arrays with different shapes");
		}

		std::swap(data_, other.data_);
	}

	std::array< size_type, N > get_sizes() { return sizes_; }

	const std::array< size_type, N >& get_sizes() const { return sizes_; }

	pointer data() { return data_; }

  private:
	pointer data_;
	std::array< size_type, N > sizes_;
	std::array< size_type, N > strides_;
	size_type total_size_;

	template< size_t Offset >
	std::array< size_type, N - Offset > slice_sizes() const
	{
		std::array< size_type, N - Offset > result{};
		std::copy(sizes_.begin() + Offset, sizes_.end(), result.begin());
		return result;
	}

	template< size_t Offset >
	std::array< size_type, N - Offset > slice_strides() const
	{
		std::array< size_type, N - Offset > result{};
		std::copy(strides_.begin() + Offset, strides_.end(), result.begin());
		return result;
	}
};
