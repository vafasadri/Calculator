using SharpCalc.Components;
using SharpCalc.Operators;

namespace SharpCalc.DataModels;

public interface IFunction : IDataModel
{
    string IMathNode.TypeName => "Function";
    public int ParameterCount { get; }
    public Scalar? Run(ReadOnlySpan<IMathNode> paramlist);
    public Scalar Differentiate(ReadOnlySpan<IMathNode> parameter);
    public Scalar? Reverse(Scalar factor, Scalar target,ReadOnlySpan<IMathNode> paramlist);
    public IFunction GetDerivative();
    IMathNode? IMathNode.SimplifyInternal() => null;
    public Complex RunFast(ReadOnlySpan<Complex> paramlist);
}
