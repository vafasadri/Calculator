using SharpCalc.Components;
using SharpCalc.DataModels;
using System.ComponentModel.DataAnnotations;

namespace SharpCalc.Operators.Arithmetic;

internal class Power : SingleOperatorBase
{
	internal Real Base => (Real)Left!;
	internal Real Exponent => (Real) Right!;
	public static readonly SingleOperatorMetadata MetadataValue = new(	
		"Power", 0,
		(left, right) => new Power((Real) left,(Real) right),
		new ISimplification[]
		{
			new PowerNumberNumber(),
			new PowerWordNumber(),
			new PowerNumberMultiply(),
			//new PowerNumberAdd(),
			new PowerPowerWord(),	
			new PowerMultiplyWord()
		}
	);
	
	public override SingleOperatorMetadata Metadata => MetadataValue;
	public override string ToText()
	{
		return $"{WrapMember(Base)}^{WrapMember(Exponent)}";
	}		
	public override Real Differentiate()
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
		var realExp = new Multiply(Exponent, new FunctionCall(StaticDataBank.ln, Base));
		mul.AddOperand(realExp.Differentiate());
		mul.Seal();
		return mul;
	}

    public override Real Reverse(Real factor, Real target)
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
			return new FunctionCall(StaticDataBank.log, new Tuple(target, Base));
		}
		else if (eqBase)
		{
			// 1/ exp
			var oneOverExp = new Power(Exponent, new Number(-1));
			return new Power(target, oneOverExp);
		}
		else throw new Exception();    
    }

    public Power(Real left, Real right) : base(left, right) { }
}
file class PowerNumberNumber : IRealSimplification<Number, Number, Power>
{
	public Real? Simplify(Number left, Number right)
	{		
		return new Number(
			ComplexMath.Pow(left.Value, right.Value)
			);
	}
}
file class PowerWordNumber : IRealSimplification<Real, Number, Power>
{
	public Real? Simplify(Real left, Number right)
	{
		if (right.Value == 0) return new Number(1);
		else if (right.Value == 1) return left;
		else return null;
	}
}
file class PowerNumberMultiply : IRealSimplification<Number, Multiply, Power>
{
	public Real? Simplify(Number left, Multiply right)
	{
		if (right.Coefficient == 1) return null;
		var allbutNumber = right.Factors.Where(n => n is not Number).Cast<Real>();
		return new Power(new Number(ComplexMath.Pow(left.Value, right.Coefficient)), new Multiply(allbutNumber));
	}
}
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
file class PowerPowerWord : IRealSimplification<Power, Real, Power>
{
    public Real? Simplify(Power left, Real right)
    {
        return new Power(left.Base, new Multiply(left.Exponent, right));
    }
}
file class PowerMultiplyWord : IRealSimplification<Multiply,Real,Power>
{
    public Real? Simplify(Multiply left, Real right)
    {       
        return new Multiply(left.Factors.Select(n => new Power((Real) n,right)));
    }
}
