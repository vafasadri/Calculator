using SharpCalc.Components;
using SharpCalc.DataModels;
using SharpCalc.Equations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCalc.Operators.Logic;
internal class Not : SingleOperatorBase,Statement
{
    public static readonly new SingleOperatorMetadata Metadata = new(typeof(Not), [new NotNullBoolean(),new NotNullAnd(),new NotNullNot(),new NotNullOr()]) { 
        Name= "Not",
        Precedence = 5,
        Creator = (left, right) => new Not((Statement)right),
        OperandsSwappable = false,
        AssociatesLeft = false
    };
    public Statement Operand => (Statement) base.Right!;
    public override string Render()
    {
        return $"~{WrapMember(Operand)}";
    }

    public override bool Equals(IMathNode? other)
    {
        if (other is Not not && Equator.Equals(not.Operand, this.Operand)) return true;
        return false;
    }
    public Not(Statement expression) : base(null,expression) { 
    
    }   
}
file class NotNullBoolean : ISimplification<IMathNode, Bool, Not>
{
    public IMathNode? Simplify(IMathNode left, Bool right)
    {
        if (right.Value) return Bool.False;
        else return Bool.True;
    }
}
file class NotNullAnd : ISimplification<IMathNode, And, Not>
{
    public IMathNode? Simplify(IMathNode left, And right)
    {
        return new Or(right.Factors.Select(n => new Not((Statement)n)));
    }
}
file class NotNullNot : ISimplification<IMathNode, Not, Not>
{
    public IMathNode? Simplify(IMathNode left, Not right)
    {
        return right.Operand;
    }
}
file class NotNullOr : ISimplification<IMathNode, Or, Not>
{
    public IMathNode? Simplify(IMathNode left, Or right)
    {
        return new And(right.Factors.Select(n => new Not((Statement)n)));
    }
}

