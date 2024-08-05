using SharpCalc.Components;
using SharpCalc.DataModels;

namespace SharpCalc.Operators;
internal abstract class SingleOperatorBase : OperatorBase
{
    public IMathNode? Left { get; }
    public IMathNode? Right { get; }
    public new SingleOperatorMetadata Metadata => (SingleOperatorMetadata) base.Metadata;
    public override  IMathNode? SimplifyInternal()
    {
        bool simpLeft = false, simpRight = false;
        var left = Left?.Simplify(out simpLeft);
        var right = Right?.Simplify(out simpRight);
        var m = Metadata.TrySimplify(left, right);
        if (m != null) return m; 
        else if (simpLeft || simpRight) return Metadata.CreateInstance(left, right);
        else return null;
    }
    
    protected SingleOperatorBase(IMathNode? left, IMathNode? right)
    {
        Left = left;
        Right = right;
    }
}
internal abstract class ScalarSingleOperator : SingleOperatorBase,IScalarOperator
{
    public new Scalar? Left => (Scalar?)base.Left;
    public new Scalar? Right => (Scalar?) base.Right;
    public Scalar? GetParentFactor(Variable variable)
    {
        if (ReferenceEquals(Left, variable) || Left is IScalarOperator op && op.GetParentFactor(variable) != null) return Left;
        if (ReferenceEquals(Right, variable) || Right is IScalarOperator op2 && op2.GetParentFactor(variable) != null) return Right;
        return null;
    }
    public virtual void EnumerateVariables(ISet<Variable> variables)
    {
        Left?.EnumerateVariables(variables);
        Right?.EnumerateVariables(variables);
    }
    public bool ContainsVariable(Variable variable) => ((Left)?.ContainsVariable(variable) ?? false) || ((Right)?.ContainsVariable(variable) ?? false);
    public abstract Complex ComputeNumerically();
    public abstract Scalar Differentiate();
    public abstract Scalar? Reverse(Scalar factor, Scalar target);
    public ScalarSingleOperator(Scalar? left, Scalar? right) : base(left,right)
    {
     
    }
}