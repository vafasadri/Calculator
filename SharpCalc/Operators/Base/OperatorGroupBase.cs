using SharpCalc.Components;
using SharpCalc.DataModels;
using System.Buffers;
using System.Collections.Specialized;
using System.Diagnostics;

namespace SharpCalc.Operators;
internal abstract class OperatorGroupBase : OperatorBase
{
    protected static ArrayPool<IMathNode> pool = ArrayPool<IMathNode>.Create();
    private readonly List<IMathNode> _factors;
    public virtual IReadOnlyList<IMathNode> Factors => _factors;
    public bool IsSealed { get; private set; }
    public new OperatorGroupMetadata Metadata => (OperatorGroupMetadata)base.Metadata;
    public virtual IMathNode Convert(IMathNode node, Symbol symbol)
    {
        return node;
    }
    public virtual void AddOperand(IMathNode operand)
    {
        if (IsSealed) throw new Exceptions.SealedOperatorException();
        if(Metadata.UnpackSelfType && operand is OperatorGroupBase op && operand.GetType() == this.GetType())
        {
            foreach(var factor in op.Factors)
            {
                AddOperand(factor);
            }
        }
        else _factors.Add(operand);
    }
    
    
    public override  IMathNode? SimplifyInternal()
    {
        IMathNode[] simplifiedFactors = pool.Rent(_factors.Count);
        var simplified = Utilities.SimplifyAny(Factors, simplifiedFactors);  
        int length = _factors.Count;
        if (Metadata.Simplifications.Count > 0)
        {
        loopStart:;           
            for (int i = 0; i < length; i++)
            {            
                for (int j = i + 1; j < length; j++)
                {                                  
                    var trySimp = Metadata.TrySimplify(simplifiedFactors[i], simplifiedFactors[j]);
                    if (trySimp != null)
                    {
                        simplifiedFactors[i] = trySimp;
                        simplifiedFactors[j] = simplifiedFactors[length - 1];
                                              
                        simplified = true;
                        length--;
                        goto loopStart;
                    }
                }
            }
        }
        IMathNode? output = null;           
        if (length == 0) output = Metadata.EmptyValue;
        else if (length == 1) output = simplifiedFactors[0];
        else if (simplified) output =  Metadata.CreateInstance(simplifiedFactors.Take(length));
        pool.Return(simplifiedFactors);
        return output;
    }
    protected virtual void SealAction(List<IMathNode> nodes)
    { 
        
    }
    public void Seal()
    {
        SealAction(_factors);
        IsSealed = true;
        _factors.TrimExcess();
    }
    //public override Scalar? GetParentFactor(Proxy variable)
    //{
    //    foreach (var item in Factors)
    //    {
    //        if (ReferenceEquals(item, variable)) return (Scalar) item;
    //        else if(item is ScalarOperatorBase op)
    //        {
    //            var en = op.GetParentFactor(variable); 
    //            if (en != null) return op;
    //        }
    //    }
    //    return null;
    //}
    //public override bool ContainsVariable(Proxy variable)
    //{
    //    foreach(var item in Factors)
    //    {
    //        if(item is Scalar r && r.ContainsVariable(variable)) return true;
    //    }
    //    return false;
    //}
    protected OperatorGroupBase(IEnumerable<IMathNode> factors)
    {
        _factors = new();
        foreach (var item in factors)
        {
            AddOperand(item);
        }
        Seal();
    }   
    protected OperatorGroupBase()
    {
        this._factors = new List<IMathNode>();
    }
}
internal abstract class ScalarOperatorGroup : OperatorGroupBase,IScalarOperator
{
    public override void AddOperand(IMathNode operand)
    {
        if (operand is not Scalar) throw new Exceptions.CustomError("Expected a scalar value");
        base.AddOperand(operand);
    }
    public void AddOperand(Scalar operand)
    {
        base.AddOperand(operand);
    }
    public abstract Complex ComputeNumerically();
    public void EnumerateVariables(ISet<Variable> variables)
    {
        foreach (var item in Factors.OfType<Scalar>())
        {
            item.EnumerateVariables(variables);
        }
    }

    public bool ContainsVariable(Variable variable)
    {
        foreach (var item in Factors.OfType<Scalar>())
        {
            if(item.ContainsVariable(variable)) return true;
        }
        return false;
    }
    public abstract Scalar Differentiate();
    public abstract Scalar? Reverse(Scalar factor, Scalar target);
    public Scalar? GetParentFactor(Variable variable)
    {
        foreach (var item in Factors.OfType<Scalar>())
        {
            if (item.ContainsVariable(variable)) return item;
        }
        throw new Exception();
    }

    protected ScalarOperatorGroup(IEnumerable<Scalar> factors) : base(factors)
    { 
        
    }
    protected ScalarOperatorGroup()
    {

    }
}