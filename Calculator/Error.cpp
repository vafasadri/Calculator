#include <Strings.hpp>
#include "Error.hpp"
 std::string Error::Text() const {
	switch (m_code)
	{
	case Error::Codes::DivisionByZero:
		return "Division by zero";
	case Error::Codes::OverFlow:
		return "Value is too large";
	case Error::Codes::UnderFlow:
		return "Value is too small";
	case Error::Codes::ExpectedIdentifier:		
		return "Expected an identifier "+ m_detail1;
	case Error::Codes::UnsolvedExpression:
		return "Failed to solve the expression";
	case Error::Codes::InsufficientParameters:
		return "Not enough parameters are provided";	
	case Error::Codes::ExpectedName:
		return "Expected a name";
	case Error::Codes::UnknownSymbol:
		return "Unknown Symbol: " + m_detail1;
	case Error::Codes::EvenRootOfNegative:
		return "Undefined";
	case Codes::UnexpectedCloseSign:
		return "Unexpected Closing Symbol";
	default:
		return "Unknown Error";
		break;
	}
}
std::string Error::Details() const {
	switch (m_code)
	{
	case Error::Codes::DivisionByZero:
		return "Division by zero is undefined in mathematics";		
	case Error::Codes::OverFlow:
		return "Value is greater than " + m_detail1;
	case Error::Codes::UnderFlow:
		return  "Value is smaller than " + m_detail1;
	case Error::Codes::ExpectedIdentifier:
		return "Expected an identifier, '" + m_detail2 + "' provided";
	case Error::Codes::UnsolvedExpression:
		return "Failed to solve the expression";
	case Error::Codes::InsufficientParameters:
		return "Expected " + m_detail1 + " parameter(s)," + m_detail2 + " provided!";
	case Error::Codes::ExpectedName:
		return "Expected a name " + m_detail1 + ", '" + m_detail2 + "' provided";
	case Error::Codes::UnknownSymbol:
		return "Unable to translate symbol: " + m_detail1;
	case Codes::UnexpectedCloseSign:
		return "Unexpected Closing Symbol : '" + m_detail1 + "'";
	default:
		return "Unknown Error";		
	}
}
