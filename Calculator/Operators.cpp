#include "Operator.hpp"

constexpr bool iscontent(Word& _in) {
	
	//return _in.Type() != TecType::Symbol && 
	//	// or converts into content
	//	(_in.Type() != TecType::Operator || (_in.Value().Operator->GetInfo().Precedence < in->GetInfo().Precedence && !_in.Value().Operator->GetInfo().UsesLeft))
	//	&& _in.Type() != TecType::Void;
	return false;
}
/// <summary>
/// converts symbols into operators
/// </summary>
/// <returns>an operator handle if a new operator has to be inserted</returns>
const MathParser::Operator* MathParser::GetOperator(Word* l, Word* c) {	
	
	if (c != nullptr && c->Type() == TecType::Symbol) {
		const Operator* m = nullptr;
		switch (c->Value().Symbol)
		{
		case Symbols::Plus:
			m = add;
			break;
		case Symbols::Minus:
			m = subtract;
			break;
		case Symbols::Cross:
			m = multiply;
			break;
		case Symbols::Slash:
			m = divide;
			break;
		case Symbols::Power:
			m = power;
			break;
		/*case Symbols::Equal:
			m = ::equal;
			break;
		case Symbols::GreaterOrEqual:
			m = greaterequal;
			break;
		case Symbols::SmallerOrEqual:
			m = smallerEqual;
			break;
		case Symbols::Smaller:
			m = smaller;
			break;
		case Symbols::Greater:
			m = ::greater;
			break;
		case Symbols::NonEqual:
			m = nonequal;
			break;*/
		/*case Symbols::Point:
			m = in;*/
			break;
		}
		if (m != nullptr) {
			*c = Word{ TecType::Operator,m };
		}
		return nullptr;
	}
	else if (l && c && l->Type() == TecType::Function && c->Type() == TecType::Expression) {
		return functionCall;
	}
	else if (c && c->Type() == TecType::Expression) {
		return bracket;
	}
	/*else if (l && c && iscontent(*l) && iscontent(*c)) {
		return in;
	}	*/
	return nullptr;
}
Number Root(Number a, Number b);

int MathParser::RunOperators(int level, expression& exp) {
	int count = 0;
	for (expression::iterator i = exp.begin(); i != exp.end(); i++)
	{		
		const Operator& op = *i->Value().Operator;
		if (i->Type() != TecType::Operator || op.GetInfo().Precedence != level) continue;
		expression::iterator start = i,
			end = i;
		end++;
		Word* left;
		Word* right;
		bool gotLeft = true, gotRight = true;
		if (start == exp.begin()) {
			left = nullptr;
			gotLeft = false;
		}
		else {
			left = &*(--start);
		}

		if (end == exp.end()) {
			right = nullptr;
			gotRight = false;
		}
		else right = &*(end);

		Word opOutput;
		op.Solve(left, right, opOutput);
		if (left != nullptr && gotLeft) ++start;
		if (right != nullptr && gotRight) --end;
		i = Replace(exp, { start,end }, opOutput);
		count++;
	}
	return count;
}
