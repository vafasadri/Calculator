#include "Operator.hpp"
class Bracket : public Operator{
public:	
	MathParser::opResult Solve(Word*&, Word*& x, Word& out) const override {	
		if (x->Type() != TecType::Expression) return MathParser::opResult::Failure;
		out =  MathParser::DoMath(*x->Value().Child);	
		x = nullptr;
		return MathParser::opResult::Success;
	}	
	MathParser::OperatorInfo& GetInfo() const override {
		static MathParser::OperatorInfo info{
			"Bracket",-1,false,true,-1
		};
		return info;
	}
};

Bracket _bracket;
extern opHandle bracket = &_bracket;
