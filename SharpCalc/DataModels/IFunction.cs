using SharpCalc.Operators;

namespace SharpCalc.DataModels;

internal interface IFunction : IMathNode, IDataModel
{
    string IMathNode.TypeName => "Function";
    public int ParameterCount { get; }
    public Real Run(IReadOnlyList<IMathNode> paramlist);
    public Real? TryRun(IReadOnlyList<IMathNode> paramlist);
    public Real Differentiate(Real parameter);
    public Real Reverse(Real factor, Real target,IReadOnlyList<IMathNode> paramlist);

    
}
