#include "Symbol.hpp"
#include <vector>
#include "Error.hpp"
#include <string.h>
Symbols Symbol::Get(const char* in, int& MoveSize)
{
	MoveSize = 1;
	switch (*in)
	{
	case '+':
		return Symbols::Plus;
	case '-':
		return Symbols::Minus;
	case '*':
		return Symbols::Cross;
	case '.':
		return Symbols::Point;
	case '/':
	case '\\':
		return Symbols::Slash;
	case '=':
	case '<':
	case '>':
	case '!':
		MoveSize = 2;
		if (strncmp(in, ">=", 2) == 0 || strncmp(in, "=>", 2) == 0) return Symbols::GreaterOrEqual;
		else if (strncmp(in, "<=", 2) == 0 || strncmp(in, "=<", 2) == 0) return Symbols::SmallerOrEqual;
		else if (strncmp(in, "==", 2) == 0) return Symbols::Equal;
		else if (strncmp(in, "!=", 2) == 0 || strncmp(in, "=!", 2) == 0) return Symbols::NonEqual;
		else if (MoveSize = 1, *in == '<') return Symbols::Smaller;
		else if (*in == '>') return Symbols::Greater;
		else if (*in == '=') return Symbols::Assign;
		else throw Error(Error::Codes::UnknownSymbol, in);;
	case '(':
		return Symbols::BracketOpen;
	case ')':
		return Symbols::BracketClose;
	case '^':
		return Symbols::Power;
	case ',':
		return Symbols::Comma;	
	default:
		throw Error(Error::Codes::UnknownSymbol, std::string(1, *in));
		break;
	}
}
const char* Symbol::ToString(Symbols in) {
	switch (in)
	{
	case Symbols::Void:
		return "";
	case Symbols::Plus:
		return "+";
	case Symbols::Minus:
		return "-";
	case Symbols::Cross:
		return "*";
	case Symbols::Slash:
		return "/";
	case Symbols::Equal:
		return "==";
	case Symbols::Assign: 
		return "=";
	case Symbols::Greater: 
		return ">";
	case Symbols::GreaterOrEqual: 
		return ">=";
	case Symbols::Smaller:        
		return "<";
	case Symbols::SmallerOrEqual: 
		return "<=";
	case Symbols::NonEqual:
		return "!=";
	case Symbols::BracketOpen:
		return "(";
	case Symbols::BracketClose:
		return ")";
	case Symbols::Power:       
		return "^";
	case Symbols::Comma:
		return ",";
	default:
		throw std::exception("corrupted symbol");
		break;
	}
}