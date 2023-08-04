#include <Enumerable.hpp>
#include "MathParser.hpp"

size_t MathParser::ParseWord(std::stack<MathParser::expression*>& out, const char* CStr, size_t strSize, int type) {
	std::string str(CStr, strSize);
	switch (type)
	{
	case 0:
		return strSize;
	case 1:
		out.top()->push_back(MathParser::Word{
		TecType::Number,
		std::stold(CStr)
			});
		return strSize;
	case 2:
	{
		IFunction* func;
		MathParser::Memory::const_iterator var;
		if ((func = FindFunction(str)) != nullptr) {
			out.top()->push_back(Word{
			TecType::Function,
			func
				});
		}		
		else if ((var = m_memory.find(str)) != m_memory.end()) {
			TecValue value;
			value.Variable = (Variable*)&*var;
			out.top()->push_back(Word{
			TecType::Variable,
			value
				});
		}
		else {
			out.top()->push_back(Word{
			TecType::Abstract,
			new string(str)
				,true });
		}
	}
	return strSize;
	case 3:
	{
		if (*CStr == '(') {
			expression* n = new expression;

			Word f{ TecType::Expression,n,true };
			out.top()->push_back(f);
			out.push(n);
			return 1;
		}
		else if (*CStr == ')') {
			if (out.size() <= 1) throw Error(Error::Codes::UnexpectedCloseSign,")");
			out.pop();
			return 1;
		}		
		else {
			int move;
			Symbols symbol = Symbol::Get(CStr, move);
			out.top()->push_back(Word{ TecType::Symbol,symbol });
			return move;
		}
	}
	default:
		return 0;
	}
}
int GetCharType(const char* leftBorder, const char* current, const char* rightBorder) {
	char c = *current;
	if (isspace(c)) return 0;
	else if (isdigit(c)) return 1;
	else if (c == '.') {
		const char* next = current + 1;
		const char* back = current - 1;
		if (next >= rightBorder || back <= leftBorder) return 3;
		int nextType = GetCharType(leftBorder, next, rightBorder);
		int backType = GetCharType(leftBorder, back, rightBorder);
		if (nextType == 1 && backType == 1) return 1;
		else return 3;
	}
	else if (isalpha(c) || c == '_')  return 2;
	else if (ispunct(c)) return 3;
	throw Error(Error::Codes::UnknownSymbol);
}

void MathParser::PushOperators(MathParser::expression& x) {
	if (x.empty()) return;
	MathParser::expression::iterator left, right;
	left = right = x.begin();
	Word* leftp = nullptr, * rightp = &*right;	
	for (; left != x.end();)
	{
		const MathParser::Operator* op = MathParser::GetOperator(leftp, rightp);
		
		if (op != nullptr) {
			x.insert(right, Word{ TecType::Operator,op });
		}
		if (leftp != nullptr && leftp->Type() == TecType::Expression) {
			PushOperators(*leftp->Value().Child);
		}
		left = right;
		leftp = rightp;
		if (right != x.end() && ++right != x.end()) {
			rightp = &*right;
		}
		else rightp = nullptr;					
	}
}

MathParser::expression MathParser::Translate(const string& m) {
	expression base;
	std::stack<expression*> d;
	d.push(&base);
	const char* start = m._Unchecked_begin();
	const char* end = m._Unchecked_end();
	// points to the first character of the current word
	const char* wordStart = start;
	int lastType = -1;
	for (const char* i = start; i < end; i++)
	{
		int currentType = GetCharType(start - 1, i, end);
		if (lastType != currentType && lastType != -1) {
			while (wordStart < i)
			{
				wordStart += ParseWord(d, wordStart, i - wordStart, lastType);
			}
		}
		lastType = currentType;
	}
	ParseWord(d, wordStart, end - wordStart, lastType);
	PushOperators(base);
	return base;
}