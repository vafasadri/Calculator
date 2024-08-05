using SharpCalc.Components;
using SharpCalc.DataModels;
using System.ComponentModel.DataAnnotations;

namespace SharpCalc.Operators.Arithmetic;

internal class Power : ScalarSingleOperator
{
	internal Scalar Base => Left!;
	internal Scalar Exponent => Right!;
	public static readonly new SingleOperatorMetadata Metadata = new(typeof(Power), [
			new PowerNumberNumber(),
			new PowerAnyNumber(),
			new PowerNumberMultiply(),
			//new PowerNumberAdd(),
			new PowerPowerAny(),
			new PowerMultiplyAny()
		])
	{

		Name = "Power",
		Precedence = 0,
        Creator = (left, right) => new Power((Scalar)left, (Scalar)right),
		OperandsSwappable = false,		
    };			
	public override string Render()
	{
		return $"{WrapMember(Base)}^{WrapMember(Exponent)}";
	}		
	public override Scalar Differentiate()
	{
		//var e = Exponent.ContainsVariable(x);
		//var b = Base.ContainsVariable(x);
		//if(e & b)
		//{

		//	throw new NotImplementedException();
		//}
		//else if (b)
		//{
		//	var exp = new Add(Exponent, new Number(-1));
		//	var der = Base.Derivative(x);
		//	return new Multiply(new Power(Base, exp), der,Exponent); 
		//}
		//      else
		//      {
		//	var ln = new FunctionCall(StaticDataBank.ln, Base);
		//	var der = Exponent.Derivative(x);
		//	return new Multiply(der,ln,this);
		//      }
		//      throw new NotImplementedException();

		var mul = new Multiply();
		mul.AddOperand(this);
		var realExp = new Multiply([Exponent, new FunctionCall(StaticDataBank.ln, Base)]);
		mul.AddOperand(realExp.Differentiate());
		mul.Seal();
		return mul;
	}

    public override Scalar Reverse(Scalar factor, Scalar target)
    {
		var eqBase = Equator.Equals(factor, Base);
		var eqExp = Equator.Equals(factor, Exponent);
		if (eqBase && eqExp)
		{
			throw new NotImplementedException();
		}
		else if (eqExp)
		{
			// log(target,exponent)
			ReadOnlySpan<IMathNode> param = [target, Base];

            return new FunctionCall(StaticDataBank.log, param);
		}
		else if (eqBase)
		{
			// 1/ exp
			var oneOverExp = new Power(Exponent, new Number(-1));
			return new Power(target, oneOverExp);
		}
		else throw new Exception();    
    }

	public override Complex ComputeNumerically() => ComplexMath.Pow(Base.ComputeNumerically(), Exponent.ComputeNumerically());
    public override bool Equals(IMathNode? other)
	{
		if (other is not Power power) return false;
		return Equator.Equals(power.Base, Base) && Equator.Equals(power.Exponent, Exponent);

	}

    public Power(Scalar left, Scalar right) : base(left, right) { }
}
// 5^2 = 25
file class PowerNumberNumber : ISimplification<Number, Number, Power>
{
	public  IMathNode? Simplify(Number left, Number right)
	{		
		return new Number(
			ComplexMath.Pow(left.Value, right.Value)
			);
	}
}
// x^1 = x
// x^0 = 1
file class PowerAnyNumber : ISimplification<Scalar, Number, Power>
{
	public  IMathNode? Simplify(Scalar left, Number right)
	{
		if (right.Value == 0) return new Number(1);
		else if (right.Value == 1) return left;
		else return null;
	}
}
// 2^(2x) = 4^x
file class PowerNumberMultiply : ISimplification<Number, Multiply, Power>
{
	public  IMathNode? Simplify(Number left, Multiply right)
	{
		if (right.Coefficient == 1) return null;
		var allbutNumber = right.Factors.Where(n => n is not Number).Cast<Scalar>();
		return new Power(new Number(ComplexMath.Pow(left.Value, right.Coefficient)), new Multiply(allbutNumber));
	}
}
// 2 ^ (x+2) = 4 * 2^x
//file class PowerNumberAdd : ISimplification<Number, Add, Power>
//{
//	public Real? Simplify(Number left, Add right)
//	{
//		if (right.NumberFactor.Value == 0) return null;
//		var allbutNumber = right.Factors.Where(n => n is not Number).Cast<Real>();

//		return new Multiply(
//			new Number(Math.Pow(left.Value, right.NumberFactor.Value)),
//			new Power(left, new Add(allbutNumber))
//		);
//	}
//}
// (a^b)^c = a^(b.c)
file class PowerPowerAny : ISimplification<Power, Scalar, Power>
{
    public  IMathNode? Simplify(Power left, Scalar right)
    {
        return new Power(left.Base, new Multiply([left.Exponent, right]));
    }
}
// (xy)^2 = x^2.y^2
file class PowerMultiplyAny : ISimplification<Multiply,Scalar,Power>
{
    public  IMathNode? Simplify(Multiply left, Scalar right)
    {       
        return new Multiply(left.Factors.Select(n => new Power((Scalar) n,right)));
    }
}
