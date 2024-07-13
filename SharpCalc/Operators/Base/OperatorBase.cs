using SharpCalc.DataModels;
using SharpCalc.Operators.Arithmetic;
using System.Dynamic;

namespace SharpCalc.Operators;
internal abstract class OperatorBase : Real
{
    public abstract OperatorMetadata Metadata { get; }
    string IMathNode.TypeName => Metadata.Name;
    public abstract Real? GetParentFactor(Proxy variable);
    public abstract Real? Simplify();
    public abstract string ToText();  
    
    protected string WrapMember(IMathNode w)
    {
        if (w is OperatorBase o && o.Metadata.Precedence >= Metadata.Precedence || 
            (w is Number{IsNegative:true } && Metadata.Precedence < Add.MetadataValue.Precedence))
            return $"({w.ToText()})";

        else return w.ToText();
    }
    public abstract Real Differentiate();   
    public abstract void EnumerateVariables(ISet<Proxy> variables);
    public abstract Real Reverse(Real factor, Real target);
    public abstract bool ContainsVariable(Proxy variable);
}