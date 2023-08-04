#include "MathParser.hpp"
std::string ToString(long double in) {
	constexpr long long maxIndex = 14;
	long double integral, decimalPart, decimalBackup;

	decimalPart = decimalBackup = modfl(abs(in), &integral);
	long long integralPart = (long long)integral;
	char a[20]{},
		b[30]{};
	size_t j = maxIndex;
	do
	{
		a[j--] = (integralPart % 10) + '0';
		integralPart /= 10;
	} while (integralPart != 0);
	int i = 0;
	unsigned long long multiplier = 1;
	while (decimalPart != floor(decimalPart)) {
		// everything after this is corrupted
		if (i >= 14 - (maxIndex - j)) break;
		decimalPart = decimalBackup * (multiplier *= 10);
		b[i++] = (static_cast<long long>(decimalPart) % 10) + '0';
	}
	std::string result;
	if (in < 0) result += '-';
	result += a + j + 1;
	if (i > 0) {
		result += '.';
		result += b;
	}
	return result;
}

//bool TryParse(const std::string& m, long double& out) {
//	size_t multiplier = 1;
//	long long buffer = 0;
//	long double power = 1;
//	for (auto i = m.rbegin(); i != m.rend(); i++, multiplier *= 10)
//	{
//		if (*i == '.') {
//			if (i == m.rbegin() || i == (m.rend() - 1))
//				return false;
//			else if (power == 1)
//				power = multiplier;
//			else return false;
//		}
//		else if (isdigit(*i)) {
//			buffer += static_cast<long long>(*i - '0') * multiplier;
//
//		}
//		else return false;
//	}
//	out = buffer / power;
//	return true;
//}

const char* MathParser::TypeToString(TecType m) {
	switch (m)
	{
	case TecType::Void:
		return "Void";
	case TecType::Symbol:
		return "Symbol";
	case TecType::Boolean:
		return "Boolean";
	case TecType::Number:
		return "Number";
	case TecType::Variable:
		return "Variable";
	case TecType::Function:
		return "Function";	
	case TecType::Abstract:
		return "Abstract Name";
	default:
		throw std::exception("corrupted enum");
		break;
	}
}

