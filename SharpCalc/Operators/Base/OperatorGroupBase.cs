using SharpCalc.Components;
using SharpCalc.DataModels;

namespace SharpCalc.Operators;
internal abstract class OperatorGroupBase : OperatorBase
{
    protected readonly List<IMathNode> _factors;
    public IReadOnlyList<IMathNode> Factors => _factors;   
    public bool IsSealed { get; private set; }
    public abstract override OperatorGroupMetadata Metadata { get; }
    public abstract IMathNode Convert(IMathNode word, Symbol symbol);
    public void AddOperand(IMathNode operand)
    {
        if (IsSealed) throw new Exceptions.SealedOperatorException();
        _factors.Add(operand);
    }
    //protected abstract void AddOperand(Word operand);
    private IMathNode? TrySimplifyFactors(IMathNode left, IMathNode right)
    {
        foreach (var item in Metadata.Simplifications)
        {

            if (item.LeftOperandType.IsInstanceOfType(left) && item.RightOperandType.IsInstanceOfType(right))
            {
                var ret = item.Simplify(left, right);
                if (ret != null) return ret;
            }
            if (item.LeftOperandType.IsInstanceOfType(right) && item.RightOperandType.IsInstanceOfType(left))
            {
                var ret = item.Simplify(right, left);
                if (ret != null) return ret;
            }
        }
        return null;
    }
    public override void EnumerateVariables(ISet<Proxy> variables)
    {
        foreach (var item in Factors.OfType<Real>())
        {
            item.EnumerateVariables(variables);
        }
    }
    public override Real? Simplify()
    {
        var simplified = Utilities.SimplifyAny(Factors, out IMathNode[] simpleFact);
        var simplifiedFactors = simpleFact.ToList();
        if (Metadata.Simplifications.Any())
        {
        loopStart:;
            for (int i = 0; i < simplifiedFactors.Count; i++)
            {
                for (int j = 0; j < simplifiedFactors.Count; j++)
                {
                    if (j == i) continue;
                    var left = (Real)simplifiedFactors[i];
                    var right = (Real)simplifiedFactors[j];
                    var trySimp = TrySimplifyFactors(left, right);
                    if (trySimp != null)
                    {
                        simplifiedFactors[i] = trySimp;
                        simplifiedFactors.RemoveAt(j);
                        simplified = true;
                        goto loopStart;
                    }
                }
            }
        }


        if (simplifiedFactors.Count == 0) return Metadata.EmptyValue;
        else if (simplifiedFactors.Count == 1) return (Real)simplifiedFactors[0];
        else if (simplified) return Metadata.CreateInstance(simplifiedFactors);
        else return null;
    }    
    public virtual void Seal()
    {
        IsSealed = true;
        _factors.TrimExcess();
    }
    public override Real? GetParentFactor(Proxy variable)
    {
        foreach (var item in Factors)
        {
            if (ReferenceEquals(item, variable)) return (Real) item;
            else if(item is OperatorBase op)
            {
                var en = op.GetParentFactor(variable); 
                if (en != null) return op;
            }
        }
        return null;
    }
    public override bool ContainsVariable(Proxy variable)
    {
        foreach(var item in Factors)
        {
            if(item is Real r && r.ContainsVariable(variable)) return true;
        }
        return false;
    }
    protected OperatorGroupBase(IEnumerable<IMathNode> factors)
    {
        this._factors = factors.ToList();
        Seal();
    }
    protected OperatorGroupBase()
    {
        this._factors = new List<IMathNode>();
    }
}