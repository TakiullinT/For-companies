#pragma once

#include <iterator>

template< typename T >

class NDArrayIterator
{
  public:
	using value_type = T;
	using pointer = T*;
	using reference = T&;
	using difference_type = std::ptrdiff_t;	
	using iterator_category = std::random_access_iterator_tag;

	NDArrayIterator(pointer ptr) : ptr_(ptr) {}

	reference operator*() const { return *ptr_; }

	pointer operator->() const { return ptr_; }

	reference operator[](difference_type n) const { return *(ptr_ + n); }

	NDArrayIterator& operator++()
	{
		ptr_++;
		return *this;
	}

	NDArrayIterator operator++(int)
	{
		NDArrayIterator temp = *this;
		ptr_++;
		return temp;
	}

	NDArrayIterator& operator--()
	{
		ptr_--;
		return *this;
	}

	NDArrayIterator operator--(int)
	{
		NDArrayIterator temp = *this;
		ptr_--;
		return temp;
	}

	NDArrayIterator& operator+=(difference_type n)
	{
		ptr_ += n;
		return *this;
	}

	NDArrayIterator operator+(difference_type n) const { return NDArrayIterator(ptr_ + n); }

	NDArrayIterator& operator-=(difference_type n)
	{
		ptr_ -= n;
		return *this;
	}

	NDArrayIterator operator-(difference_type n) const { return NDArrayIterator(ptr_ - n); }

	difference_type operator-(const NDArrayIterator& other) const { return ptr_ - other.ptr_; }

	bool operator==(const NDArrayIterator< T >& other) const { return ptr_ == other.ptr_; }
	bool operator!=(const NDArrayIterator< T >& other) const { return !(*this == other); }
	bool operator<(const NDArrayIterator< T >& other) const { return ptr_ < other.ptr_; }
	bool operator<=(const NDArrayIterator< T >& other) const { return ptr_ <= other.ptr_; }
	bool operator>(const NDArrayIterator< T >& other) const { return ptr_ > other.ptr_; }
	bool operator>=(const NDArrayIterator< T >& other) const { return ptr_ >= other.ptr_; }

  private:
	pointer ptr_;
};
