using SharpCalc.Components;
using SharpCalc.DataModels;
using System.Diagnostics.Tracing;
using System.Text;

namespace SharpCalc.Operators.Arithmetic;
internal class Add : OperatorGroupBase
{
    public readonly static OperatorGroupMetadata MetadataValue = new(
        "Addition", 3,
        () => new Add(),
        (factors) => new Add(factors.Cast<Real>()),
        new ISimplification[]
        {
            new AddNumberNumber(),
            new AddNumberWord(),
            new AddWordWord(),
            new AddMultiplyWord(),
            new AddMultiplyMultiply(),
            new AddWordAdd()
        },
        new Number(0),
        new Number(0),
        true
    );
    public override OperatorGroupMetadata Metadata => MetadataValue;
    public override string ToText()
    {
        StringBuilder builder = new();
        bool IsFirst = true;
        foreach (var item in Factors)
        {
            var factor = WrapMember((Real)item);
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
    public override IMathNode Convert(IMathNode word, Symbol symbol)
    {
        return symbol switch
        {
            Symbol.Null => word,
            Symbol.Plus => word,
            Symbol.Minus => Negative.Create((Real)word),
            _ => throw new Exception()
        };
    }

    public override Real Differentiate()
    {
        return new Add(Factors?.Select(n => ((Real)n).Differentiate()) ?? Enumerable.Empty<Real>());
    }

    public override Real Reverse(Real factor, Real target)
    {
        var allbutFactor = Factors.Exclude(Enumerable.Repeat(factor, 1)).Cast<Real>();
        var negAll = allbutFactor.Select(Negative.Create);
        return new Add(negAll.Append(target));
    }
    public Add(IEnumerable<Real> words) : base(words) { }
    public Add(params Real[] words) : base(words) { }
    public Add() : base() { }
    static double rate(Real target)
    {
        if (target is Proxy)
        {
            return 0;
        }
        else if (target is Multiply m && m.IsNegative) return 2;
        else return 1;

    }
    public override void Seal()
    {
        _factors.Sort((left, right) =>
        {
            var rl = rate((Real)left);
            var rr = rate((Real)right);
            if (rl != rr) return rl.CompareTo(rr);

            if (left is INamed ln && right is INamed rn)
            {
                return ln.Name.CompareTo(rn.Name);
            }
            return 0;
        });
        base.Seal();
    }
}
// x + 0 = x
file class AddNumberWord : IRealSimplification<Number, Real, Add>
{
    public Real? Simplify(Number left, Real right)
    {
        if (left.Value == 0) return right;
        else return null;
    }
}
file class AddWordAdd : IRealSimplification<Real, Add, Add>
{
    public Real? Simplify(Real left, Add right)
    {
        return new Add(right.Factors.Append(left).Cast<Real>());
    }
}
// x + x = 2x
file class AddWordWord : IRealSimplification<Real, Real, Add>
{
    public Real? Simplify(Real left, Real right)
    {
        if (Equator.Instance.Equals(left, right))
        {

            var mul = new Multiply(left, new Number(2));

            return mul;
        }
        return null;
    }
}
// 2 + 2 = 4
file class AddNumberNumber : IRealSimplification<Number, Number, Add>
{
    public Real? Simplify(Number left, Number right)
    {
        return new Number(left.Value + right.Value);
    }
}
// 2x + x = 3x
file class AddMultiplyWord : IRealSimplification<Multiply, Real, Add>
{
    public Real? Simplify(Multiply left, Real right)
    {
        if (!left.Factors.Cast<Real>().Contains(right, Equator.Instance)) return null;
        var once = true;
        // removing right from left but only once
        var except = left.Factors.Cast<Real>().Where(n =>
        {
            if (Equator.Equals(n, right))
            {
                var onceBackup = once;
                once = false;
                return !onceBackup;
            }
            else return true;
        });
        return new Multiply(new Add(new Multiply(except), new Number(1)), right);
    }
}
// 2x + yx = x * (2+y)
file class AddMultiplyMultiply : IRealSimplification<Multiply, Multiply, Add>
{
    public Real? Simplify(Multiply left, Multiply right)
    {
        var (leftDistinct, intersect, rightDistinct) = left.Factors.Cast<Real>().Intersection<Real>(right.Factors.Cast<Real>(), Equator.Instance);
        if (!intersect.Any()) return null;

        var add = new Add(new Multiply(leftDistinct), new Multiply(rightDistinct));
        return new Multiply(intersect.Append(add));
    }
}
