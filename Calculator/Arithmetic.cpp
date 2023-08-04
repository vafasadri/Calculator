#include "Operator.hpp"

enum necessityState : long long
{
	unused,
	unnecessary,
	necessary
};

class ArithmeticOp : public MathParser::Operator {
private:
	struct  DerivedInfo : public MathParser::OperatorInfo
	{	
		Number(*Action)(Number, Number);		
		necessityState leftState;
		necessityState rightState;		
	};
	static DerivedInfo MakeInfo(necessityState leftPos, necessityState rightPos,
	int precedence,const string& name, Number(*Method)(Number, Number)) {	
		DerivedInfo r;
		r.Action = Method;
		r.UsesLeft = leftPos != unused;
		r.UsesRight = rightPos != unused;
		r.Precedence = precedence;
		r.leftState = leftPos;
		r.rightState = rightPos;	
		r.Name = name;
		return r;
	}
	static ArithmeticOp::DerivedInfo infos[];
	DerivedInfo* Info;
	constexpr MathParser::OperatorInfo& GetInfo() const { return *Info; } 
public:
	enum class code {
		add,
		subtract,
		multiply,
		divide,
		/*equal,*/
		/*smaller,
		smallerequal,
		greater,
		greaterequal,
		nonequal,*/
		/*factorial,*/
		power,
		root
	};
	ArithmeticOp(code c) : Info(infos + (int) c) {}
	
	MathParser::opResult Solve(Word*& left, Word*& right, Word& out) const override {
		long double nleft = 0,
			nright = 0,
			result = 0;
		MathParser::opResult leftType = MathParser::opResult::Failure,
			rightType = MathParser::opResult::Failure;
		if (left != nullptr) {
			leftType = left->ConvertToNumber(nleft);
		}

		if (right != nullptr) {
			rightType = right->ConvertToNumber(nright);
		}

		necessityState needLeft = Info->leftState,
			needRight = Info->rightState;

		if ((needLeft != unused && leftType == MathParser::opResult::Exception) ||
			(needRight && rightType == MathParser::opResult::Exception)) return MathParser::opResult::Exception;
		if (needLeft && leftType != MathParser::opResult::Failure) {
			left = nullptr;
		}
		if (needRight  && rightType != MathParser::opResult::Failure) {
			right = nullptr;
		}		
		// if a needed operand is missing
		if ((needLeft == necessary && leftType == MathParser::opResult::Failure) ||
			(needRight == necessary && rightType == MathParser::opResult::Failure))
			return MathParser::opResult::Failure;
		// operate				
		result = Info->Action(nleft, nright);
		out = MathParser::Word{ TecType::Number,result};
		return MathParser::opResult::Success;
	}
};
ArithmeticOp::DerivedInfo ArithmeticOp::infos[] = {
		MakeInfo(unnecessary, necessary,3,"Add",[](Number a,Number b) {return a + b; }), 
		MakeInfo(unnecessary, necessary,3,"Subtract",[](Number a,Number b) {return a - b; }) ,
		MakeInfo(necessary, necessary,2,"Multiply", [](Number a,Number b) { return a * b; }), 
		MakeInfo(necessary, necessary,2,"Divide", [](Number a,Number b) { return a / b; }), 
		/*MakeInfo(necessary, necessary,4,"Equals", [](Number a,Number b) { return Number(a == b); }), */
	/*	MakeInfo(necessary, necessary,4,"Smaller",[](Number a,Number b) {return a < b; }),
		MakeInfo(necessary, necessary,4,"SmallerEqual", MathParser::TecType::Boolean),
		MakeInfo(necessary, necessary,4,"Greater", MathParser::TecType::Boolean), 
		MakeInfo(necessary, necessary,4,"Greater Or Equal", MathParser::TecType::Boolean), 
		MakeInfo(necessary, necessary,4,"Non-Equal", MathParser::TecType::Boolean), */
		//MakeInfo(necessary, unused), /*factorial = */
		MakeInfo(necessary, necessary,0,"Power",[](Number a,Number b) { return pow(a,b); }), /*power = */
		MakeInfo(necessary, necessary,0,"Root",[](Number a,Number b) { return pow(a,1/b); }) /*root = */
};
extern opHandle
add =  new ArithmeticOp(ArithmeticOp::code::add),
subtract = new ArithmeticOp(ArithmeticOp::code::subtract),
multiply = new ArithmeticOp(ArithmeticOp::code::multiply),
divide = new ArithmeticOp(ArithmeticOp::code::divide),
power = new ArithmeticOp(ArithmeticOp::code::power),
root = new ArithmeticOp(ArithmeticOp::code::root);

