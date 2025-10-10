#pragma once

#include <initializer_list>
#include <type_traits>

#include <array>
#include <stdexcept>

template< typename T, std::size_t N >
class InitializerParser
{
  public:
	using size_type = std::size_t;

	struct ParseResult
	{
		std::array< size_type, N > sizes;
		T* data;
		size_type total_size;

		~ParseResult() { delete[] data; }
	};

	template< typename NestedList >
	ParseResult parse(const NestedList& list)
	{
		std::array< size_type, N > sizes{};
		size_type total_size = calculate_size(list, sizes, 0);
		T* data = new T[total_size];

		size_type index = 0;
		flatten(list, data, index, sizes, 0);

		return { sizes, data, total_size };
	}

  private:
	size_type calculate_size(const std::initializer_list< T >& list, std::array< size_type, N >& sizes, size_type level)
	{
		if (level != N - 1)
		{
			throw std::invalid_argument("InitializerParser: incorrect nesting level at leaf");
		}
		if (sizes[level] == 0)
		{
			sizes[level] = list.size();
		}
		else if (sizes[level] != list.size())
		{
			throw std::invalid_argument("InitializerParser: inconsistent dimensions at leaf");
		}
		return list.size();
	}

	template< typename List >
	size_type calculate_size(const List& list, std::array< size_type, N >& sizes, size_type level)
	{
		if (level >= N)
		{
			throw std::invalid_argument("InitializerParser: too many nested levels");
		}

		if (sizes[level] == 0)
		{
			sizes[level] = list.size();
		}
		else if (sizes[level] != list.size())
		{
			throw std::invalid_argument("InitializerParser: inconsistent dimensions");
		}

		if (level == N - 1)
		{
			return list.size();
		}

		size_type total = 0;
		for (const auto& sub : list)
		{
			total += calculate_size(sub, sizes, level + 1);
		}
		return total;
	}

	void flatten(const std::initializer_list< T >& list, T* data, size_type& index, const std::array< size_type, N >&, size_type level)
	{
		if (level != N - 1)
		{
			throw std::invalid_argument("InitializerParser: incorrect nesting level in flatten leaf");
		}
		for (const auto& item : list)
		{
			data[index++] = item;
		}
	}

	template< typename List >
	void flatten(const List& list, T* data, size_type& index, const std::array< size_type, N >& sizes, size_type level)
	{
		if (level >= N - 1)
		{
			throw std::invalid_argument("InitializerParser: too many nested levels in flatten");
		}
		for (const auto& sub : list)
		{
			flatten(sub, data, index, sizes, level + 1);
		}
	}
};
