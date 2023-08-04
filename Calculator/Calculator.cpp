// Calculator.cpp : This file contains the 'main' function. Program execution begins and ends there.
//
#include <iostream>
#include "MathParser.hpp"
#include "GC.h"
using namespace std;

int main()
{
	srand((unsigned int) time(0));
	std::cout << std::boolalpha << "Welcome, This program is designed to solve math expressions\n";
	MathParser runner;
	while (true) {
		std::cout << ">>> ";
		string value;
		std::cin.clear();
		std::getline(std::cin, value);
		if (value.empty()) {
			continue;
		}
		try {
			MathParser::Word result = runner.Run(value);
			string out = result.Print();
			if (out != "") {
				cout << out << endl;
			}			
		}
		catch (const Error& err) {
			std::cout << err.Details() << std::endl;
		}
	}	
	return 0;
}
