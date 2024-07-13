using SharpCalc.DataModels;
using SharpCalc.Operators;

namespace SharpCalc.Equations;
internal class Equation : Operators.SingleOperatorBase
{
    public string TypeName => "Equation";
    public static readonly SingleOperatorMetadata MetadataValue = new(
        "Equation",
        5,
        (left, right) => new Equation((Real)left, (Real)right),
        new ISimplification[]
        {
            new EqualsNumberNumber(),
            new EqualsProxyReal(),
            new EqualsOperatorReal(),
            
        }
        );
    public override SingleOperatorMetadata Metadata => MetadataValue;
    public override string ToText()
    {
        return $"{Left!.ToText()} = {Right!.ToText()}";
    }
    public Equation(Real left, Real right) : base(left, right) { }
    internal enum AddResult
    {
        Added, Exists, Conflicts
    };
    public override Real Differentiate()
    {
        throw new NotImplementedException();
    }

    public override void EnumerateVariables(ISet<Proxy> variables)
    {
        throw new NotImplementedException();
    }

    public override Real Reverse(Real factor, Real target)
    {
        throw new NotImplementedException();
    }

    public override Real GetParentFactor(Proxy variable)
    {
        throw new NotImplementedException();
    }
}
file class EqualsNumberNumber : ISimplification<Number, Number, Equation>
{
    public IMathNode? Simplify(Number left, Number right)
    {
        //return null;
        return new DataModels.Boolean(left.Value == right.Value);
    }
}
file class EqualsProxyReal : IRealSimplification<Proxy, Real, Equation>
{
    public Real? Simplify(Proxy left, Real right)
    {
        left.Value = right;
        return new EquationResult(left.Name, right);
    }
}
file class EqualsOperatorReal : IRealSimplification<OperatorBase, Real, Equation>
{
    public Real? Simplify(OperatorBase left, Real right)
    {
        var set = new HashSet<Proxy>();
        left.EnumerateVariables(set);
        var x = set.First();
        Real value = left;
        while (value is OperatorBase op)
        {
            var factor = op.GetParentFactor(x);

            right = op.Reverse(factor, right);
            value = factor;
        }
        var proxy = (Proxy)value;
        var checkSet = new HashSet<Proxy>();
        right.EnumerateVariables(checkSet);
        if (!checkSet.Contains(proxy))
        proxy.Value = right;
        return new EquationResult(proxy.Name, right);
    }
}
