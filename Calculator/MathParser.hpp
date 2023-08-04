

#pragma once
#include <vector>
#include <map>
#include <string>
#include <list>
#include <stack>
#include <deque>
#include <Strings.hpp>
#include "Error.hpp"
#include "Symbol.hpp"
#include "Utilities.hpp"

using std::map;
using std::vector;
using std::string;

class MathParser {
public:
	enum class TecType : unsigned char
	{
		// primary types:
		Void, Symbol,Boolean, Number, Variable, Function, Operator, Expression,Abstract,Parameter
	};
	enum class ContentType {
		Boolean,RealNumber,Integer,NaturalNumber,Function,ParameterList,
	};
	enum class opResult {
		Unqualified,Failure,Exception,Success
	};
	class Word;
	struct Parameter;
	using Number = long double;	
	using Variable = std::pair<const string, Number>;
	using Memory = map<string, Number>;
	using expression = std::list<Word>;	
	using Range = std::pair<expression::iterator, expression::iterator>;

	// an interface for functions like sine & cosine
	class IFunction {
	public:
		string Name;
		virtual size_t ParameterCount() const = 0;
		virtual Word Run(const vector<Word>& params) const = 0;
		virtual string Print() = 0;
		virtual ~IFunction() = default;
	};
		
	struct OperatorInfo;
	// a math operator like addition or subtaction
	 class Operator {
	 public:
		 
		virtual opResult Solve(Word*& left,Word*& right,Word& result) const = 0;
		virtual OperatorInfo& GetInfo() const = 0;		
	};
	 struct OperatorInfo
	 {
	 private:
		 static long long GetId() {
			 static long long index = 0;
			 return index++;
		 }
	 public:		
		 string Name;
		 int Precedence;
		 bool UsesLeft = false;
		 bool UsesRight = false;
		 long long OppositeID = -1;
		 const long long OperatorID = GetId();
	 };
	// Represents anything this app works with
	union TecValue
	{
		long long Void = 0;
		Symbols Symbol;
		MathParser::Number Number;
		Variable* Variable;
		MathParser::IFunction* Function;		
		const MathParser::Operator* Operator;
		const Parameter* Parameter;
		string* Text;
		expression* Child;
		TecValue() = default;
		constexpr TecValue(Symbols in) : Symbol(in) {}
		constexpr TecValue(MathParser::Number in) : Number(in) {}
		constexpr TecValue(MathParser::Variable* in) : Variable(in) {}
		constexpr TecValue(decltype(Function) in) : Function(in) {}
		constexpr TecValue(string* in) : Text(in) {}
		constexpr TecValue(const MathParser::Operator* in) : Operator(in) {}
		constexpr TecValue(MathParser::expression* in) : Child(in){}
		constexpr TecValue(const MathParser::Parameter* in) : Parameter(in) {}
		bool operator==(const TecValue&) = delete;
	};
	union ContentValue
	{
		bool Boolean;
		unsigned long long NaturalNumber;
		long long Integer;
		long double RealNumber;
	};
	class Word {
	private:
		char reserved = 0;
		TecType type{};
		short flags = 0;
		int gcToken = -1;
		TecValue value{};
	public:
		Word() = default;
		Word(const TecType& a, const TecValue& b, bool GC = false);
		Word(const Word&);
		~Word();
		Word& operator=(const Word& right);
		bool operator==(const Word& right) const;
		constexpr TecType Type() const {
			return type;
		}
		constexpr TecValue Value() const {
			return value;
		}		
		std::string ToString() const;
		std::string Print() const;
		std::string ValueToString() const;
		const string* GetName() const;

		opResult ConvertToNumber(Number& out) const;
		opResult ConvertToNatural(unsigned long long& out) const;
		opResult ConvertToBoolean(bool& a) const;
		opResult ConvertToInteger(long long& out) const;
		opResult ConvertTo(MathParser::ContentType _type, MathParser::ContentValue& out) const;
		bool Is(ContentType) const;
	};
	struct Parameter {
		string name;
		Word value;
		bool supplied;
	};
private:				
	// Fields
	std::deque<IFunction*> m_runtimeFunctions;
	static const std::vector<IFunction*> s_builtinFunctions;
	Memory m_memory{
		Variable("pi",3.14159265358979323846),
		Variable("e",2.71828182845904523536)
	};
	// Methods		
	static const Operator* GetOperator(Word* l, Word* c);
	static void PushOperators(MathParser::expression& x);
	static int RunOperators(int level, expression& Crunch);	
	size_t ParseWord(std::stack<expression*>& out,const char* CStr, size_t strSize, int type);	
	bool CreateFunction(const MathParser::expression& expression);
	static Word DoMath(expression x);
	expression Translate(const string& m);
	
	static inline expression ToExpression(const Word& in) {		
		if (in.Type() == TecType::Expression) return *in.Value().Child;
		else if (in.Type() == TecType::Void) return expression();
		else return expression{ in };
	}
	static inline Word ToWord(const expression& in) {
		if (in.empty()) {
			return Word();
		}
		else if (in.size() == 1) {
			return in.front();
		}
		else return Word{ TecType::Expression,new expression(in),true };					
	}
	[[nodiscard]]
	inline static MathParser::expression::iterator Replace(expression& cont, Range r, const Word& replaceWith) {
		*r.first = replaceWith;
		return --cont.erase(++r.first,
			++r.second);
	}	
public:	
	IFunction* FindFunction(const std::string&);
	static const char* TypeToString(TecType m);	
	MathParser();
	~MathParser();
	Word Run(const string&);
	friend class Function;
	friend class Bracket;
	friend class IFunction;
	friend class FunctionCall;
};






