#pragma once
#include <string>
#include <exception>
class Error
{
public:
	enum class Codes
	{
		DivisionByZero,
		OverFlow,
		UnderFlow,
		ExpectedIdentifier,
		ExpectedName,
		UnsolvedExpression,
		InsufficientParameters,
		EvenRootOfNegative,
		UnknownSymbol,
		UnexpectedCloseSign
	};
private:
	Codes m_code;
	std::string m_detail1, m_detail2;	
public:
	
	std::string Text() const; 
	std::string Details() const;
	inline Error(Codes errorCode,std::string detail1 = "",std::string detail2 = "")  {
		m_code = errorCode;
		m_detail1 = detail1;
		m_detail2 = detail2;
	}
	constexpr Codes Code() const{
		return m_code;
	}
};

