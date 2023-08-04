#include <list>
#include "MathParser.hpp"

MathParser::Word MathParser::DoMath(expression x) {
	// brackets
	//for (expression::iterator i = x.begin(); i != x.end(); i++)
	//{
	//	if (i->Type() != TecType::Expression) continue;
	//	Word out{ TecType::Number,0.0 };
	//	expression::iterator left = i;
	//	Word solved = DoMath(*i->Value().Child);
	//	if (solved.Type() == TecType::Expression) *i = solved;
	//	else if (solved.Type() == TecType::Void) *i = Word{ TecType::Expression,new expression,true };
	//	else *i = Word{ TecType::Expression,new expression{solved},true };
	//	// try calling functions
	//	opResult funcResult;
	//	if (i != x.begin() && ((funcResult = CallFunction(*--left, *i->Value().Child, out)) != opResult::Unqualified || (funcResult = CallFunction(*left, *i->Value().Child, out)) != opResult::Unqualified)) {
	//		if (funcResult == opResult::Failure) {
	//			throw Error(Error::Codes::UnsolvedExpression);
	//		}
	//		else if (funcResult == opResult::Exception) continue;
	//		else i = Replace(x, Range{ left,i }, out);
	//	}
	//	// well maybe it's just a normal bracket
	//	else {
	//		expression& inn = *i->Value().Child;
	//		if (inn.size() == 1) {
	//			*i = inn.front();
	//		}
	//		else if (inn.empty()) {
	//			i = x.erase(i);
	//		}
	//		// hidden multiplication, example : 2(2 + 1) = 2 * (2 + 1)
	//		Number p;
	//		if (i != x.begin() && left->Evaluate(p) != opResult::Failure) {
	//			x.insert(i, Cross);
	//		}
	//	}
	//}
	// Run the operators
	for (int i = -1; i <= 4; i++)
	{
		// i is the precedence of the operators supposed to run in this loop
		RunOperators(i, x);
		
	}

	return ToWord(x);
}