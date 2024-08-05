using SharpCalc.Components;
using SharpCalc.DataModels;
using SharpCalc.Operators.Arithmetic;
using System.Dynamic;

namespace SharpCalc.Operators;
internal abstract class OperatorBase : IMathNode
{
    private OperatorMetadata? _metadata;
    public OperatorMetadata Metadata { 
        get
        {
            if (_metadata == null) _metadata = OperatorMetadata.GetMetadata(this.GetType()); 
            return _metadata;
        }
    }
    public string TypeName => Metadata.Name;
    public abstract bool Equals(IMathNode? other);
    public abstract IMathNode? SimplifyInternal();
    public abstract string Render();
    /// <summary>
    /// Puts parentheses around a node if necessary
    /// </summary>
    /// <param name="w"></param>
    /// <returns></returns>
    protected string WrapMember(IMathNode w)
    {
        if (w is OperatorBase o && o.Metadata.Precedence >= Metadata.Precedence || 
            (w is Number n && ((n.Value.a != 0 && n.Value.b  != 0) || n.Value.a < 0 || n.Value.b < 0) && Metadata.Precedence < Add.Metadata.Precedence))
            return $"({w.Render()})";

        else return w.Render();
    }
}
internal interface IScalarOperator : Scalar
{    
    public Scalar? Reverse(Scalar factor, Scalar target);
    public Scalar? GetParentFactor(Variable variable);
}