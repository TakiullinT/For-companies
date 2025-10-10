# all: main

# main: main.cpp
# 	clang-format -i *.cpp -style="file"
# 	clang main.cpp -o main

all: test_runner

test_runner: src/quat.cpp test/tests.cpp
	clang-format -i include/*.h src/*.cpp test/*.cpp -style="file"
#	g++ src/*.cpp test/*.cpp -I include -I /usr/local/include -L /usr/local/lib -lgtest -lgtest_main -pthread -o test_runner
