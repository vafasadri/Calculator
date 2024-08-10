using SharpCalc.Components;
using SharpCalc.DataModels;
using System.Diagnostics.Tracing;
using System.Text;

namespace SharpCalc.Operators.Arithmetic;
internal class Add : ScalarOperatorGroup
{
    public readonly static new OperatorGroupMetadata Metadata = new(typeof(Add), [
        new AddNumberNumber(),
            new AddAnyAny(),
            new AddNumberAny(),
            new AddMultiplyAny(),            
            new AddMultiplyMultiply(),
            ])
    {
        Name = "Addition",
        Precedence = 3,
        EmptyCreator = () => new Add(),
        FullCreator = (factors) => new Add(factors.Cast<Scalar>()),       
        UnpackSelfType = true,
        EmptyValue = new Number(0),
        IsLeftOptional = true,        
    };
          
    public override string Render()
    {
        StringBuilder builder = new();
        bool IsFirst = true;
        foreach (var item in Factors)
        {
            var factor = WrapMember((Scalar)item);
            if (!IsFirst && !factor.StartsWith('-'))
            {
                builder.Append(" + ");
            }
            else if (!IsFirst)
            {
                builder.Append(" - ");
                factor = factor.TrimStart(' ', '-');
            }
            builder.Append(factor);
            IsFirst = false;
        }
        return builder.ToString();
    }
    public override IMathNode Convert(IMathNode value, Symbol symbol)
    {
        return symbol switch
        {
            Symbol.Null => value,
            Symbol.Plus => value,
            Symbol.Minus => Negative.Create((Scalar)value),
            _ => throw new Exception()
        };
    }

    public override Scalar Differentiate()
    {
        return new Add(Factors?.Select(n => ((Scalar)n).Differentiate()) ?? Enumerable.Empty<Scalar>());
    }

    public override Scalar Reverse(Scalar factor, Scalar target)
    {
        var allbutFactor = Factors.Exclude([factor],Equator.Instance).Cast<Scalar>();
        var negAll = allbutFactor.Select(Negative.Create);
        return new Add(negAll.Append(target));
    }
    public Add(IEnumerable<Scalar> factors) : base(factors) { }
    public Add() : base() { }
    static double rate(Scalar target)
    {
        if (target is Variable)
        {
            return 0;
        }
        else if (target is Multiply m && m.IsNegative) return 2;
        else return 1;
    }
    protected override void SealAction(List<IMathNode> nodes)
    {
       
        nodes.Sort((left, right) =>
        {
            var rl = rate((Scalar)left);
            var rr = rate((Scalar)right);
            if (rl != rr) return rl.CompareTo(rr);

            if (left is INamed ln && right is INamed rn)
            {
                return ln.Name.CompareTo(rn.Name);
            }
            return 0;
        });          
    }

    public override Complex ComputeNumerically()
    {
        Complex result = new(0);
        foreach (var item in Factors)
        {
            result += ((Scalar)item).ComputeNumerically();
        }
        return result;
    }

    public override bool Equals(IMathNode? other) => false;
}
// x + 0 = x
file class AddNumberAny : ISimplification<Number, Scalar, Add>
{
    public  IMathNode? Simplify(Number left, Scalar right)
    {
        if (left.Value == 0) return right;
        else return null;
    }
}
// x + x = 2x
file class AddAnyAny : ISimplification<Scalar, Scalar, Add>
{
    public  IMathNode? Simplify(Scalar left, Scalar right)
    {
        if (Equator.Equals(left, right))
        {

            var mul = new Multiply([left, new Number(2)]);
            return mul;
        }
        return null;
    }
}
// 2 + 2 = 4
file class AddNumberNumber : ISimplification<Number, Number, Add>
{
    public  IMathNode? Simplify(Number left, Number right)
    {
        return new Number(left.Value + right.Value);
    }
}
// yx + x = x(y+1)
file class AddMultiplyAny : ISimplification<Multiply, Scalar, Add>
{
    public  IMathNode? Simplify(Multiply left, Scalar right)
    {
        if (!left.Factors.Cast<Scalar>().Contains(right, Equator.Instance)) return null;
        var once = true;
        // removing right from left but only once
        var except = left.Factors.Cast<Scalar>().Where(n =>
        {
            if (once && Equator.Equals(n, right))
            {              
                once = false;
                return false;
            }
            else return true;
        });
        return new Multiply([new Add([new Multiply(except), new Number(1)]), right]);
    }
}
// 2x + yx = x * (2+y)
file class AddMultiplyMultiply : ISimplification<Multiply, Multiply, Add>
{
    public  IMathNode? Simplify(Multiply left, Multiply right)
    {
        var (leftDistinct, intersect, rightDistinct) = left.Factors.Cast<Scalar>().Intersection<Scalar>(right.Factors.Cast<Scalar>(), Equator.Instance);
        if (!intersect.Any()) return null;       
        var add = new Add([new Multiply(leftDistinct), new Multiply(rightDistinct)]);
        return new Multiply(intersect.Append(add));
    }
}
