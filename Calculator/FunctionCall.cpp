#include <Enumerable.hpp>
#include "Operator.hpp"
using namespace std;
class FunctionCall : public MathParser::Operator {
	static MathParser::OperatorInfo info;
public:
	static vector<MathParser::Word> MakeParameters(expression exp) {
		exp = MathParser::ToExpression(MathParser::DoMath(exp));
		vector<expression> ParamSplit = MyLib::Split<Word, expression, expression>(exp, Word{ MathParser::TecType::Symbol,Symbols::Comma });
		vector<Word> m;
		for (expression& exp : ParamSplit) {
			if (exp.size() > 1) {
				m.push_back({ MathParser::TecType::Expression,new expression(exp),true });
			}
			else if (exp.size() == 1) {
				m.push_back(exp.front());
			}
			else if (exp.size() == 0) {
				m.push_back(Word());
			}
		}
		return m;
	}
	
	MathParser::opResult Solve(Word*& header, Word*& parameters, Word& out) const override {
		const MathParser::IFunction& ptr = *header->Value().Function;
		vector<Word> ParamReady = MakeParameters(*parameters->Value().Child);
		out = ptr.Run(ParamReady);
		header = nullptr;
		parameters = nullptr;
		return MathParser::opResult::Success;
	}
	MathParser::OperatorInfo& GetInfo() const override {
		static MathParser::OperatorInfo info{
	"Function Call",-1,true,true,-1
		};
		return info;
	}	
};

FunctionCall _functionCall;
opHandle functionCall = &_functionCall;