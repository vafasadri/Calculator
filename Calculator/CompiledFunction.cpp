#define _USE_MATH_DEFINES
#include <cmath>
#include <SizedPointer.hpp>
#include "UsingMathParser.hpp"
class CompiledFunction : public MathParser::IFunction {
	using T = MathParser::Number;
	using NoArg = T(*)();
	using Arg1 = T(*)(T);
	using Arg2 = T(*)(T, T);
	using Arg3 = T(*)(T, T, T);
	using Arg4 = T(*)(T, T, T, T);
	using Arg5 = T(*)(T, T, T, T, T);
	void* Method;
	size_t parameters;
public:
	struct PropertyList {
		std::string FullName;
		std::string Description = "description unavailable";
		std::vector<std::string> ParameterDescription;
	} Properties;
	constexpr CompiledFunction(string _name, NoArg _Func, const PropertyList& p = {}) : Method(_Func), Properties(p), parameters(0) { Name = _name; }
	constexpr CompiledFunction(string _name, Arg1 _Func, const PropertyList& p = {}) : Method(_Func), Properties(p), parameters(1) { Name = _name; }
	constexpr CompiledFunction(string _name, Arg2 _Func, const PropertyList& p = {}) : Method(_Func), Properties(p), parameters(2) { Name = _name; }
	constexpr CompiledFunction(string _name, Arg3 _Func, const PropertyList& p = {}) : Method(_Func), Properties(p), parameters(3) { Name = _name; }
	constexpr CompiledFunction(string _name, Arg4 _Func, const PropertyList& p = {}) : Method(_Func), Properties(p), parameters(4) { Name = _name; }
	constexpr CompiledFunction(string _name, Arg5 _Func, const PropertyList& p = {}) : Method(_Func), Properties(p), parameters(5) { Name = _name; }
	size_t ParameterCount() const override { return parameters; }
	MathParser::Word Run(const std::vector<MathParser::Word>& Params) const override;
	~CompiledFunction() override = default;
	string Print() override;
};
using unit = MathParser::Number;
constexpr unit factorial(unit input) {
	unit temp = 1;
	for (unit i = 1; i <= input; i++, temp *= i);
	return  temp;
}

inline unit Radian(unit a) {
	return  a * (M_PI / 180.0);
}

inline unit RandomUnit() {
	return (unit)rand();
}

unit sqrtFunc(unit a) {
	if (a < 0) throw Error(Error::Codes::EvenRootOfNegative);
	return sqrt(a);
}

unit cot(unit a) {
	return 1 / tan(a);
}

unit Root(unit a, unit b) {
	return std::pow(a, 1.0 / b);;
}

CompiledFunction
_abs{ "abs",abs,{"Absolute Value","The absolute value of x"} },
_sin{ "sin",sin,{"Sine","Sine of x radians"} },
_tan{ "tan",tan,{"Tangent","Tangent of x radians"} },
_cos{ "cos",cos,{"Cosine","Cosine of x radians"} },
_cot{ "cot",cot,{"Cotangent","Cotangent of x radians"} },
_rad{ "rad",Radian,{"Degree to Radian","Radian equivalent of x degrees"} },
_sqrt{ "sqrt",sqrtFunc,{"Square Root","Square root of x"} },
_fact{ "fact",factorial,{"Factorial","Factorial of x"} },
_rand{ "rand",RandomUnit,{"Random Number","a randomly generated integer"} },
_log{ "log",log,{"Logarithm","Natural logarithm of x"} },
_remainder{ "remainder",remainder,{"Remainder","the Remainder of x divided by y"} },
_round{ "round",round,{"Round","The value of x rounded to the nearest integral"} },
_floor{ "floor",floor,{"Floor","The value of x rounded downward"} },
_max{ "max",fmaxl,{"Maximum","The maximum of x and y"} },
_min{ "min",fminl,{"Minimum","The minimum of x and y"} },
_cbrt{ "cbrt",cbrt,{"Cubic Root","The cubic root of x"} },
_root{ "root",Root,{"N'th Root","N't root of x",{"Radical expression ","Radical index"}} },
_ceil{ "ceil",ceil,{"Ceil","The smallest integer greater than or equal to x"} };

const std::vector<MathParser::IFunction*> MathParser::s_builtinFunctions{
	&_abs,&_sin,&_tan,&_cos,&_cot,&_rad,&_sqrt,
		&_fact,&_rand,&_log,&_remainder,&_round,
		&_floor,&_max,&_min,&_cbrt,&_root,&_ceil
};

MathParser::Word CompiledFunction::Run(const std::vector<MathParser::Word>& Params) const {
	// if not enough parameters are provided
	if (Params.size() < parameters) {
		throw Error(Error::Codes::InsufficientParameters,/* how much expected?*/ MyLib::DescribeNumber(parameters),/* how much provided? */ MyLib::DescribeNumber(Params.size()));
	}
	std::vector<Number> extractNumbers;
	for (size_t i = 0; i < parameters; i++)
	{
		Number evalResult;
		if (Params[i].ConvertToNumber(evalResult) != opResult::Success) {
			throw Error(Error::Codes::ExpectedIdentifier);
		}
		else extractNumbers.push_back(evalResult);
	}
	Number funcOutput;
	switch (parameters)
	{
	case 0:
		funcOutput = NoArg(Method)();
		break;
	case 1:
		funcOutput = Arg1(Method)(extractNumbers[0]);
		break;
	case 2:
		funcOutput = Arg2(Method)(extractNumbers[0], extractNumbers[1]);
		break;
	case 3:
		funcOutput = Arg3(Method)(extractNumbers[0], extractNumbers[1], extractNumbers[2]);
		break;
	case 4:
		funcOutput = Arg4(Method)(extractNumbers[0], extractNumbers[1], extractNumbers[2], extractNumbers[3]);
		break;
	case 5:
		funcOutput = Arg5(Method)(extractNumbers[0], extractNumbers[1], extractNumbers[2], extractNumbers[3], extractNumbers[4]);
		break;
	default:
		throw std::exception("corrupted delegate");
	}
	return  Word{ TecType::Number, funcOutput };
}
string CompiledFunction::Print() {
	static string alphabet[] = { "x","y","z","d","q","m"/*... i got bored*/ };

	string result = Name + '(' +
		MyLib::String<char>::Join(MyLib::SizedPointer<string>(alphabet, parameters), ", ") + ") = " +
		Properties.FullName + " : " + Properties.Description + '\n';

	for (size_t i = 0; i < Properties.ParameterDescription.size(); i++)
	{
		result += alphabet[i] + " : " + Properties.ParameterDescription[i] + '\n';
	}
	return result;
}
