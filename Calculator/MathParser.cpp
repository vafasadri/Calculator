
#include <Enumerable.hpp>
#include "MathParser.hpp"
using namespace std;
inline  MathParser::Word Assign{ MathParser::TecType::Symbol,Symbols::Assign };
MathParser::IFunction* MathParser::FindFunction(const std::string& name)
{
	
	for (IFunction* i : s_builtinFunctions)
	{
		if (i->Name == name) return i;
	}
	for (IFunction* i : m_runtimeFunctions)
	{
		if (i->Name == name) return i;
	}
	return nullptr;
}

MathParser::Word MathParser::Run(const string& value) {
	expression byteCode = Translate(value);
	if (CreateFunction(byteCode)) return Word();
	Word res = DoMath(byteCode);
	if (res.Type() != TecType::Expression) return res;
	expression& x = *res.Value().Child;
	x.push_front(Word());
	for (expression::iterator i = --x.end(); i != x.begin(); i--)
	{
		if (*i != Assign) continue;
		bool createNew = false;
		Number dump1, dump2;
		expression::iterator left = i, right = i;
		--left;
		++right;
		if (left == x.begin() || left->ConvertToNumber(dump1) != opResult::Success) {
			if (left != x.begin() && left->Type() == TecType::Abstract) {
				createNew = true;
			}
			else
				throw Error(Error::Codes::ExpectedIdentifier, "after operator '='", i == x.begin() ? "Nothing" : TypeToString(left->Type()));
		}
		if (right == x.end() || right->ConvertToNumber(dump2) != opResult::Success) {
			throw Error(Error::Codes::ExpectedIdentifier, "after operator '='", right == x.end() ? "Nothing" : TypeToString(right->Type()));
		}
		Word result;
		// the value of a variable is unchangable in mathematics, why change it?
		if (createNew) {
			// assign
			auto block = m_memory.insert_or_assign(*left->Value().Text, dump2);
			result = Word{ TecType::Variable, &*(block.first) };
		}
		else {
			// compare
			result = Word{ TecType::Boolean,*left == *right };
		}
		i = Replace(x, Range{ left,right }, result);
	}
	x.pop_front();
	return res;
}

MathParser::MathParser()
{
}
MathParser::~MathParser() {
	for (auto i : m_runtimeFunctions)
	{
		delete i;
	}
}

