#include <sstream>
#include "UsingMathParser.hpp"
static int SpaceLogic(MathParser::TecType t) {
	// 1 = numeric
	// 2 = alphabetal
	// 3 = not spaced (Symbols)
	// 4 = always spaced (Operators)
	switch (t)
	{
	case MathParser::TecType::Boolean:
	case  MathParser::TecType::Number:
		return 1;
	case  MathParser::TecType::Function:
	case  MathParser::TecType::Variable:
	case  MathParser::TecType::Abstract:
		return 2;
	case  MathParser::TecType::Symbol:
	case  MathParser::TecType::Expression:
		return 3;
	case  MathParser::TecType::Operator:
		return 4;	
	default:
		return 0;
	}
}
std::string ToString(const MathParser::expression& x) {
	std::stringstream m;
	int lastSpaced = 0;
	for (auto& i : x)
	{
		int s = SpaceLogic(i.Type());
		if (lastSpaced != 0 && (s != lastSpaced && s != 3 && lastSpaced != 3) || lastSpaced == 4 || s == 4) {
			m << ' ';
		}
		m << i.ToString();
		lastSpaced = s;
	}
	return m.str();
}
// this is used to print the math result
std::string MathParser::Word::Print() const {
	switch (type)
	{	
	case TecType::Variable:
		return value.Variable->first + " => " + ::ToString(value.Variable->second);		
	case TecType::Function:
		return value.Function->Print();
	case TecType::Operator:
		return value.Operator->GetInfo().Name;
	case TecType::Abstract:
		return *value.Text + " => ?";
	case TecType::Parameter:
		return value.Parameter->name + " => " + value.Parameter->value.ValueToString();
	case TecType::Expression:
		return ::ToString(*value.Child);
	default:
		return ToString();
	}
}
std::string MathParser::Word::ToString() const {
	switch (type)
	{
	case TecType::Void:
		return "";
	case TecType::Symbol:
		return Symbol::ToString(value.Symbol);
	case TecType::Boolean:
		return (value.Number ? "true" : "false");	
	case TecType::Number:
		return std::to_string(value.Number);
	case TecType::Variable:
		return value.Variable->first;		
	case TecType::Function:
		return value.Function->Name;			
	case TecType::Abstract:
		return *value.Text;		
	case TecType::Expression:
		return '(' + ::ToString(*value.Child) + ')';
	case TecType::Parameter:
		return value.Parameter->name;
	default:
		return "";
		break;
	}
}
std::string MathParser::Word::ValueToString() const {
	switch (type)
	{	
	case TecType::Variable:
		return std::to_string(value.Variable->second);						
	case TecType::Expression:
		return ::ToString(*value.Child);
	case TecType::Abstract:
		return "?";
	case TecType::Parameter:		
		return value.Parameter->value.ValueToString();		
	default:
		return ToString();		
	}
}

const string* MathParser::Word::GetName() const {
	switch (type)
	{
	case TecType::Abstract:
		return value.Text;		
	case TecType::Variable:
		return &value.Variable->first;		
	case TecType::Function:
		return &value.Function->Name;				
	}
	return nullptr;
}
  MathParser::Word::Word(const TecType& a, const TecValue& b, bool GC) : type(a), value(b) {
	if (GC) {
		switch (type)
		{
		case MathParser::TecType::Expression:
			OpenGC<expression>::CreateReference(value.Child);
			break;
		case MathParser::TecType::Abstract:
			OpenGC<string>::CreateReference(value.Text);
			break;
		}

	}
}
	MathParser::Word::Word(const Word& right) : type(right.type), value(right.value), gcToken(right.gcToken) {
	if (gcToken != -1)
		switch (type)
		{
		case MathParser::TecType::Expression:
			OpenGC<expression>::CopyReference(gcToken);
			break;
		case MathParser::TecType::Abstract:
			OpenGC<string>::CopyReference(gcToken);
			break;
		}
}
MathParser::Word::~Word() {
	if (gcToken != -1) {
		switch (type)
		{
		case MathParser::TecType::Expression:
			OpenGC<expression>::DestroyReference(gcToken);
			break;
		case MathParser::TecType::Abstract:
			OpenGC<string>::DestroyReference(gcToken);
			break;
		}
	}
}

Word& MathParser::Word::operator=(const Word& right) {
	if (gcToken != -1) {
		switch (type)
		{
		case MathParser::TecType::Expression:
			OpenGC<expression>::DestroyReference(gcToken);
			break;
		case MathParser::TecType::Abstract:
			OpenGC<string>::DestroyReference(gcToken);
			break;
		}
	}
	type = right.type;
	value = right.value;
	gcToken = right.gcToken;
	if (gcToken != -1) {
		switch (type)
		{
		case MathParser::TecType::Expression:
			OpenGC<expression>::CopyReference(gcToken);
			break;
		case MathParser::TecType::Abstract:
			OpenGC<string>::CopyReference(gcToken);
			break;
		}
	}
	return *this;
}

bool MathParser::Word::operator==(const Word& right) const {
	if (type != right.type) return false;
	switch (type)
	{
	case MathParser::TecType::Abstract:
		return *value.Text == *right.value.Text;
	case MathParser::TecType::Number:
		return value.Number == right.value.Number;
	case MathParser::TecType::Symbol:
		return value.Symbol == right.value.Symbol;
	case MathParser::TecType::Boolean:
		return bool(value.Number) == bool(right.value.Number);
	default:
		return value.Void == right.value.Void;
	}
}

opResult Word::ConvertToNumber(Number& out) const {
	switch (type)
	{
	case TecType::Number:
	case TecType::Boolean:
		out = value.Number;
		return opResult::Success;
	case TecType::Variable:
		out = value.Variable->second;
		return opResult::Success;
	case TecType::Parameter:
		if (value.Parameter->supplied) {
			return value.Parameter->value.ConvertToNumber(out);
		}
		else return opResult::Exception;
	case TecType::Abstract:
		return opResult::Exception;
	default:
		return opResult::Failure;
	}
}
opResult Word::ConvertToNatural(unsigned long long& out) const {
	if (type == TecType::Parameter) {
		return value.Parameter->value.ConvertToNatural(out);
	}
	return opResult::Failure;
}
opResult Word::ConvertToBoolean(bool& out) const {
	return opResult::Failure;
}
opResult Word::ConvertToInteger(long long& out) const {
	return opResult::Failure;
}
opResult Word::ConvertTo(MathParser::ContentType _type, MathParser::ContentValue& out) const {
	switch (_type)
	{
	case MathParser::ContentType::Boolean:
		return ConvertToBoolean(out.Boolean);
	case MathParser::ContentType::RealNumber:
		return ConvertToNumber(out.RealNumber);
	case MathParser::ContentType::Integer:
		return  ConvertToInteger(out.Integer);
	case MathParser::ContentType::NaturalNumber:
		break;
	case MathParser::ContentType::Function:
		break;
	case MathParser::ContentType::ParameterList:
		break;
	default:
		return opResult::Failure;		
	}
	return opResult::Success;
}
bool MathParser::Word::Is(ContentType _type) const
{
	if (type == TecType::Parameter) {
		return value.Parameter->value.Is(_type);
	}
	switch (_type)
	{
	case MathParser::ContentType::Boolean:
		return type == TecType::Boolean;
		break;
	case MathParser::ContentType::RealNumber:
		break;
	case MathParser::ContentType::Integer:
		break;
	case MathParser::ContentType::NaturalNumber:
		break;
	case MathParser::ContentType::Function:
		return type == TecType::Function;
		break;
	case MathParser::ContentType::ParameterList:
		return type == TecType::Expression;
		break;
	default:
		break;
	}

	return false;
}
