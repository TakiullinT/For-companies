#pragma once
#include "ndarray.hpp"
#include "ndarray_iterator.hpp"

#include <algorithm>
#include <array>
#include <cstddef>
#include <iterator>
#include <stdexcept>

template< typename T, std::size_t N >
class NDArray;

template< typename T, std::size_t N >
class NDArrayView
{
  public:
	using value_type = T;
	using pointer = T*;
	using const_pointer = const T*;
	using reference = T&;
	using const_reference = const T&;
	using size_type = std::size_t;
	using iterator = NDArrayIterator< T >;
	using const_iterator = NDArrayIterator< const T >;

	NDArrayView() noexcept : data_(nullptr), sizes_{}, strides_{} {}

	NDArrayView(pointer data, const std::array< size_type, N >& sizes, const std::array< size_type, N >& strides) :
		data_(data), sizes_(sizes), strides_(strides)
	{
	}

	NDArrayView(const NDArrayView&) noexcept = default;
	NDArrayView(NDArrayView&&) noexcept = default;
	NDArrayView& operator=(const NDArrayView&) noexcept = default;
	NDArrayView& operator=(NDArrayView&&) noexcept = default;
	~NDArrayView() noexcept = default;

	NDArrayView(const NDArray< T, N >& arr) noexcept : data_(arr.data()), sizes_(arr.sizes()), strides_(arr.strides())
	{
	}

	template< typename InputIt >
	NDArrayView(InputIt first, InputIt last, const std::array< size_type, N >& sizes, const std::array< size_type, N >& strides) :
		data_(&(*first)), sizes_(sizes), strides_(strides)
	{
		if (static_cast< size_type >(std::distance(first, last)) != total_count())
		{
			throw std::invalid_argument("Iterator range size mismatch");
		}
	}

	auto operator[](size_type index)
	{
		if constexpr (N == 1)
		{
			return data_[index];
		}
		else
		{
			return NDArrayView< T, N - 1 >(data_ + index * strides_[0], slice_sizes< 1 >(), slice_strides< 1 >());
		}
	}

	auto operator[](size_type index) const
	{
		if constexpr (N == 1)
		{
			return data_[index];
		}
		else
		{
			return NDArrayView< T, N - 1 >(data_ + index * strides_[0], slice_sizes< 1 >(), slice_strides< 1 >());
		}
	}

	size_type count() const { return sizes_[0]; }

	size_type total_count() const
	{
		size_type total = 1;
		for (auto s : sizes_)
		{
			total *= s;
		}
		return total;
	}

	size_type dim() const { return N; }

	pointer begin() { return data_; }

	pointer end() { return data_ + total_count(); }

	const_pointer begin() const { return data_; }

	const_pointer end() const { return data_ + total_count(); }

	const_pointer cbegin() const { return begin(); }

	const_pointer cend() const { return end(); }

	bool empty() const { return total_count() == 0; }

	bool operator==(const NDArrayView& other) const
	{
		return data_ == other.data_ && sizes_ == other.sizes_ && strides_ == other.strides_;
	}

	bool operator!=(const NDArrayView& other) const { return !(*this == other); }

	pointer data() { return data_; }

	const_pointer data() const { return data_; }

	const std::array< size_type, N >& sizes() const { return sizes_; }

	const std::array< size_type, N >& strides() const { return strides_; }

  private:
	pointer data_;
	std::array< size_type, N > sizes_;
	std::array< size_type, N > strides_;

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

template< typename T, std::size_t N >
class NDArrayConstView : public NDArrayView< const T, N >
{
  public:
	using base_type = NDArrayView< const T, N >;
	using value_type = typename base_type::value_type;
	using pointer = typename base_type::pointer;
	using const_pointer = typename base_type::const_pointer;
	using reference = typename base_type::reference;
	using const_reference = typename base_type::const_reference;
	using size_type = typename base_type::size_type;

	NDArrayConstView() noexcept = default;

	NDArrayConstView(pointer data, const std::array< size_type, N >& sizes, const std::array< size_type, N >& strides) :
		base_type(data, sizes, strides)
	{
	}

	NDArrayConstView(const NDArray< T, N >& view) : base_type(view.data(), view.sizes(), view.strides()) {}

	NDArrayConstView(const NDArrayConstView&) noexcept = default;
	NDArrayConstView(NDArrayConstView&&) noexcept = default;
	NDArrayConstView& operator=(const NDArrayConstView&) noexcept = default;
	NDArrayConstView& operator=(NDArrayConstView&&) noexcept = default;
	~NDArrayConstView() noexcept = default;

	template< typename InputIt >
	NDArrayConstView(InputIt first, InputIt last, const std::array< size_type, N >& sizes, const std::array< size_type, N >& strides) :
		NDArrayView< const T, N >(&(*first), sizes, strides)
	{
		if (static_cast< size_type >(std::distance(first, last)) != total_count())
		{
			throw std::invalid_argument("Iterator range size mismatch");
		}
	}

	using base_type::operator[];
	using base_type::begin;
	using base_type::cbegin;
	using base_type::cend;
	using base_type::count;
	using base_type::data;
	using base_type::end;
	using base_type::total_count;

	auto operator[](size_type index) const
	{
		if constexpr (N == 1)
		{
			return this->data_[index];
		}
		else
		{
			return NDArrayConstView< T, N - 1 >(
				this->data_ + index * this->strides_[0],
				this->template slice_sizes< 1 >(),
				this->template slice_strides< 1 >());
		}
	}
};
