using SharpCalc.Components;
using SharpCalc.DataModels;
using SharpCalc.Operators;

namespace SharpCalc.Equations;
internal class Equal : Operators.SingleOperatorBase, Statement
{
    public static readonly new SingleOperatorMetadata Metadata = new(typeof(Equal), [
            new EqualsNumberNumber(),
            new EqualsScalarScalar(),
            new EqualsProxyReal(),
            new EqualsOperatorScalar(),
            new EqualsFunctionScalar(),
            new EqualsFunctionStatement(),
            new EqualsScalarStatement()
        ])
    {
        Name = "Equals",
        Precedence = 5,
        Creator = (left, right) => new Equal(left, right),
        OperandsSwappable = true,
    };    
    public override string Render()
    {
        return $"{Left!.Render()} = {Right!.Render()}";
    }

    public override bool Equals(IMathNode? other) => throw new NotImplementedException();

    public Equal(IMathNode left, IMathNode right) : base(left, right) { }
}
file class EqualsNumberNumber : ISimplification<Number, Number, Equal>
{
    public IMathNode? Simplify(Number left, Number right)
    {
        var b = left.Value == right.Value;
        return b? DataModels.Bool.True : DataModels.Bool.False;       
    }
}
file class EqualsFunctionScalar : ISimplification<IFunction, Scalar, Equal>
{
    public IMathNode? Simplify(IFunction left, Scalar right)
    {
        throw new Exceptions.CustomError("Cannot Compare a function and a scalar");
    }
}
file class EqualsFunctionStatement : ISimplification<IFunction, Statement, Equal>
{
    public IMathNode? Simplify(IFunction left, Statement right)
    {
        throw new Exceptions.CustomError("Cannot Compare a function and a statement");
    }
}
file class EqualsScalarStatement : ISimplification<Scalar, Statement, Equal>
{
    public IMathNode? Simplify(Scalar left, Statement right)
    {
        throw new Exceptions.CustomError("Cannot Compare a scalar and a statement");
    }
}
file class EqualsScalarScalar : ISimplification<Scalar, Scalar, Equal>
{
    public IMathNode? Simplify(Scalar left, Scalar right)
    {

        if (Equator.Equals(left, right))
        {
            return DataModels.Bool.True;
        }
        return null;
    }
}
file class EqualsProxyReal : ISimplification<Variable, Scalar, Equal>
{
    public IMathNode? Simplify(Variable left, Scalar right)
    {
        left.Value = right;
        return new EquationResult(left, right);
    }
}
file class EqualsOperatorScalar : ISimplification<IScalarOperator, Scalar, Equal>
{
    public IMathNode? Simplify(IScalarOperator left, Scalar right)
    {

        var set = new HashSet<Variable>();
        left.EnumerateVariables(set);
        var x = set.First();
        Scalar value = left;
        while (value is IScalarOperator op)
        {
            var factor = op.GetParentFactor(x);
            var newRight = op.Reverse(factor, right);

            if (newRight == null) break;
            right = newRight;
            value = factor;
        }
        var proxy = (Variable)value;
        var checkSet = new HashSet<Variable>();
        right.EnumerateVariables(checkSet);
        if (!checkSet.Contains(proxy))
            proxy.Value = right;
        return new EquationResult(proxy, right);
    }
}
