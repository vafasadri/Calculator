using SharpCalc.Components;
using SharpCalc.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCalc.Operators.Logic;

internal class And : LogicOperatorGroup, Statement
{
    public readonly static new OperatorGroupMetadata Metadata = new(typeof(And), [new AndStatementNot(),new AndStatementStatement(),new AndStatementBoolean(),new AndStatementOr()])
    {
        Name = "Logical And",
        Precedence = 6,       
        EmptyCreator = () => new And(),
        FullCreator = (factors) => new And(factors.Cast<Statement>()),        
        UnpackSelfType = true,           
    };                             
    public override IMathNode Convert(IMathNode node, Symbol symbol) => node;
    public override bool Equals(IMathNode? other) => false;
    public override string Render()
    {
        return string.Join(" & ", Factors.Select(WrapMember));
    }
    public And() : base() { }
    public And(IEnumerable<Statement> statements) : base(statements) { }
}

file class AndStatementNot : ISimplification<Statement, Not, And>
{
    public IMathNode? Simplify(Statement left, Not right)
    {
        if (Equator.Equals(left, right.Operand)) return DataModels.Bool.False;
        return null;
    }
}
file class AndStatementStatement : ISimplification<Statement, Statement, And>
{
    public IMathNode? Simplify(Statement left, Statement right)
    {
        if (Equator.Equals(left, right)) return left;
        return null;
    }
}
file class AndStatementBoolean : ISimplification<Statement, DataModels.Bool, And>
{
    public IMathNode? Simplify(Statement left, DataModels.Bool right)
    {
        // return x
        if (right.Value == true) return left;
        // return false
        else return right;       
    }
}
file class AndStatementOr : ISimplification<Statement, Or, And>
{
    public IMathNode? Simplify(Statement left, Or right)
    {
        return new Or(from p in right.Factors select new And([(Statement) p, left]) );
    }
}


