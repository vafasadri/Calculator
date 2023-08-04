#include <Strings.hpp>
#include <SizedPointer.hpp>
#include "UsingMathParser.hpp"
#include "Operator.hpp"
using namespace MyLib;

std::string ToString(const MathParser::expression& x);

class Function : public MathParser::IFunction {
	std::vector<Parameter*> Parameters;
	expression body;

	void FixParams(MathParser::expression& x) {
		for (MathParser::Word& f : x)
		{
			if (f.Type() == MathParser::TecType::Expression) {
				FixParams(*f.Value().Child);
				continue;
			}
			else if (f.Type() == MathParser::TecType::Parameter) {
				f = { MathParser::TecType::Parameter,Parameters[f.Value().Void] };
			}
		}
	}
public:
	size_t ParameterCount() const override { return Parameters.size(); }	
	~Function() override {
		for (Parameter* i : Parameters)
		{
			delete i;
		}
	};
	Word Run(const vector<MathParser::Word>& params) const override {
		if (params.size() < Parameters.size()) {
			throw Error(Error::Codes::InsufficientParameters,/* how much expected?*/ MyLib::DescribeNumber(Parameters.size()),/* how much provided? */ MyLib::DescribeNumber(params.size()));
		}
		for (size_t i = 0; i < Parameters.size(); i++)
		{
			Parameters[i]->value = params[i];
			Parameters[i]->supplied = true;
		}
		return MathParser::DoMath(body);
	}
	
	Function(const expression& exp, const vector<string>& paramNames) {	
		for (const string& i : paramNames)
		{
			Parameters.push_back(new Parameter{ i,Word(),false });
		}
		body = exp;
		FixParams(body);
		try {
			body = MathParser::ToExpression(MathParser::DoMath(body));
		}
		catch (const Error& er)
		{
			this->~Function();
			throw er;
		}
	}

	string Print() override {
		std::vector<string> paramNames;
		for (auto i : Parameters)
		{
			paramNames.push_back(i->name);
		}
		return Name + '(' + MyLib::String<char>::Join(paramNames, ", ") + ") => " + ::ToString(body);
	}
};

void EditExpression(const vector<string>& paramNames, MathParser::expression& x) {
	for (MathParser::Word& f : x)
	{
		if (f.Type() == MathParser::TecType::Expression) {
			EditExpression(paramNames, *f.Value().Child);
			continue;
		}
		const string* name = f.GetName();
		if (name) {
			size_t ind = IndexOf(paramNames, *name);
			if (ind != -1) {
				MathParser::TecValue v;
				v.Void = ind;
				f = MathParser::Word{ MathParser::TecType::Parameter, v };
			}
		}
	}
}
bool MathParser::CreateFunction(const MathParser::expression& x) {		
	 expression::const_iterator temp = x.begin();
	if (x.size() < 3) return false;
	if (x.front().Type() != TecType::Abstract) return false;
	string name = *temp->Value().Text;
	if ((++temp)->Type() != TecType::Operator ||
		temp->Value().Operator->GetInfo().OperatorID != bracket->GetInfo().OperatorID) return false;
	if ((++temp)->Type() != TecType::Expression) return false;
	expression::const_iterator paramList = temp;
	if (temp == x.end() || *(++temp) != Word{TecType::Symbol,Symbols::Assign}) return false;
	// this is a function, proven. start initalizing

	vector<string> paramNames;
	vector<vector<Word>> parameters = MyLib::Split<Word, expression>(*paramList->Value().Child,
		Word{TecType::Symbol,Symbols::Comma});
	for (auto& i : parameters)
	{
		if (i.size() != 1) throw Error(Error::Codes::ExpectedName);
		const string* name = i.front().GetName();
		if(name != nullptr) {
			paramNames.push_back(*name);		
		}
		else throw Error(Error::Codes::ExpectedName);				
	}
	temp++;
	MathParser::expression translation{temp,x.end()};	
	EditExpression(paramNames, translation);
	
	try
	{
		Function* makefunc = new Function(translation, paramNames);
		m_runtimeFunctions.push_back(makefunc);
		return true;
	}
	catch (const Error&) {
		return false;
	}
}
 

 
 