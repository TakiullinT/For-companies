#pragma once

#include <cstddef>
#include <stdexcept>

template< typename T >
class Vector
{
  private:
	T* data;
	size_t size;
	size_t capacity;

	void resize(size_t new_capacity);

  public:
	Vector();
	~Vector();
	void push_back(const T& value);
	void pop_back();

	T& operator[](size_t index);
	const T& operator[](size_t index) const;
	Vector(const Vector< T >& other);
	Vector< T >& operator=(const Vector< T >& other);

	T* begin();
	T* end();
	const T* begin() const;
	const T* end() const;

	size_t get_size() const;

	void clear();
	Vector(size_t count, const T& default_value);
};

template< typename T >
Vector< T >::Vector(size_t count, const T& default_value) : size(count), capacity(count)
{
	data = new T[capacity];
	for (size_t i = 0; i < size; ++i)
	{
		data[i] = default_value;
	}
}

template< typename T >
Vector< T >::Vector() : size(0), capacity(1)
{
	data = new T[capacity];
}

template< typename T >
Vector< T >::~Vector()
{
	delete[] data;
}

template< typename T >
Vector< T >::Vector(const Vector< T >& other) : size(other.size), capacity(other.capacity)
{
	data = new T[capacity];
	for (size_t i = 0; i < size; ++i)
	{
		data[i] = other.data[i];
	}
}

template< typename T >
Vector< T >& Vector< T >::operator=(const Vector< T >& other)
{
	if (this == &other)
	{
		return *this;
	}

	delete[] data;
	size = other.size;
	capacity = other.capacity;
	data = new T[capacity];
	for (size_t i = 0; i < size; ++i)
	{
		data[i] = other.data[i];
	}
	return *this;
}

template< typename T >
void Vector< T >::push_back(const T& value)
{
	if (size == capacity)
	{
		resize(capacity * 2);
	}
	data[size++] = value;
}

template< typename T >
void Vector< T >::pop_back()
{
	if (size > 0)
	{
		--size;
	}
}

template< typename T >
T& Vector< T >::operator[](size_t index)
{
	if (index >= size)
	{
		throw std::out_of_range("Index out of range");
	}
	return data[index];
}

template< typename T >
const T& Vector< T >::operator[](size_t index) const
{
	if (index >= size)
	{
		throw std::out_of_range("Index out of range");
	}
	return data[index];
}

template< typename T >
T* Vector< T >::begin()
{
	return data;
}

template< typename T >
T* Vector< T >::end()
{
	return data + size;
}

template< typename T >
const T* Vector< T >::begin() const
{
	return data;
}

template< typename T >
const T* Vector< T >::end() const
{
	return data + size;
}

template< typename T >
void Vector< T >::resize(size_t new_capacity)
{
	if (new_capacity > capacity)
	{
		T* new_data = new T[new_capacity];
		for (size_t i = 0; i < size; ++i)
		{
			new_data[i] = data[i];
		}
		delete[] data;
		data = new_data;
		capacity = new_capacity;
	}
}

template< typename T >
size_t Vector< T >::get_size() const
{
	return size;
}

template< typename T >
void Vector< T >::clear()
{
	delete[] data;
	size = 0;
	capacity = 1;
	data = new T[capacity];
}

template class Vector< int >;
template class Vector< float >;
template class Vector< long long >;
