#pragma once
#include <string>

enum class Symbols : unsigned char {
	Void, Plus, Minus, Cross, Slash, Equal, Assign, Greater, GreaterOrEqual, Smaller, SmallerOrEqual, NonEqual, BracketOpen, BracketClose, Power, Comma,Point,Factorial
};
namespace Symbol {
	Symbols Get(const char* in, int& MoveSize);
	 const char* ToString(Symbols);
}
