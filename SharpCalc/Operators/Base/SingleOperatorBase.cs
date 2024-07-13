using SharpCalc.Components;
using SharpCalc.DataModels;

namespace SharpCalc.Operators;
internal abstract class SingleOperatorBase : OperatorBase
{
    public IMathNode? Left { get; }
    public IMathNode? Right { get; }
    public abstract override SingleOperatorMetadata Metadata { get; }
    public override Real? Simplify()
    {
        bool simpLeft = false, simpRight = false;
        var left = Left?.TrySimplify(out simpLeft);
        var right = Right?.TrySimplify(out simpRight);
        foreach (var item in Metadata.Simplifications)
        {
            if (item.LeftOperandType.IsInstanceOfType(left) && item.RightOperandType.IsInstanceOfType(right))
            {
                var returnValue = item.Simplify(left, right);
                if (returnValue != null) return (Real?) returnValue;
            }
        }
        if (simpLeft || simpRight) return Metadata.CreateInstance(left, right);
        else return null;
    }   
    public override Real? GetParentFactor(Proxy variable)
    {
        if (ReferenceEquals(Left, variable) || Left is OperatorBase op && op.GetParentFactor(variable) != null) return (Real) Left;
        if (ReferenceEquals(Right, variable) || Right is OperatorBase op2 && op2.GetParentFactor(variable) != null) return (Real) Right;
        return null;
    }
    public override void EnumerateVariables(ISet<Proxy> variables)
    {
        (Left as Real)?.EnumerateVariables(variables);
        (Right as Real)?.EnumerateVariables(variables);
    }
    public override bool ContainsVariable(Proxy variable) => ((Left as Real)?.ContainsVariable(variable) ?? false) || ((Right as Real)?.ContainsVariable(variable) ?? false);
    protected SingleOperatorBase(IMathNode? left, IMathNode? right)
    {
        Left = left;
        Right = right;
    }
}