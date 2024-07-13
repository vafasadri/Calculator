using SharpCalc.Components;
using SharpCalc.DataModels;
using System.Diagnostics;
using System.Text;

namespace SharpCalc.Operators.Arithmetic;
internal class Multiply : OperatorGroupBase
{
    public static readonly OperatorGroupMetadata MetadataValue = new(
       "Multiply",
       2,
       () => new Multiply(),
       (factors) => new Multiply(factors.Cast<Real>()),
       new ISimplification[]
       {
           new MultiplyNumberNumber(),
           new MultiplyNumberWord(),
           new MultiplyMultiplyWord(),
           new MultiplyPowerPower(),
           new MultiplyPowerWord(),
           new MultiplyWordWord(),
       },
       new Number(1),
       new Number(1)
    );
    public override OperatorGroupMetadata Metadata => MetadataValue;
    bool _neg;
    Complex _co;
    public bool IsNegative
    {
        get
        {
            Debug.Assert(IsSealed);
            return _neg;
        }
    }
    public Complex Coefficient
    {
        get
        {
            Debug.Assert(IsSealed);
            return _co;
        }
    }
    
    public override string ToText()
    {
        List<Real> up = new();
        Multiply? denominator = null;
        foreach (var item in Factors.Where(x => x is not Number))
        {
            if (item is Power pow && ((pow.Exponent is Number rg && rg.Value.IsReal() && rg.Value.b < 0)
                || (pow.Exponent is Multiply { IsNegative: true })))
            {
                denominator ??= new();
                var power = new Power(pow.Base, Negative.Create(pow.Exponent)).SuperSimplify(out _);
                denominator.AddOperand(power);
            }
            else
            {
                up.Add((Real)item);
            }
        }
        denominator?.Seal();

        Complex numberfactor = Coefficient;
        StringBuilder builder = new();
        if (numberfactor == -1) builder.Append('-');
        else if (Coefficient != 1 || up.Count == 0)
        {
            var real = numberfactor.IsReal();

            if (!real) builder.Append('(');
            builder.Append(numberfactor);
            if (!real) builder.Append(')');
        }

        builder.AppendJoin(" * ", up.Select(WrapMember));
        if (denominator != null)
        {
            builder.Append(" / ");
            builder.Append(denominator.ToText());
        }
        return builder.ToString();
    }
    public override IMathNode Convert(IMathNode word, Symbol symbol)
    {
        if (symbol == Symbol.Cross || symbol == Symbol.Null || symbol == Symbol.Point) return word;
        else if (symbol == Symbol.Slash) return new Power((Real)word, new Number(-1));
        else throw new Exception();
    }
    public override Real Differentiate()
    {
        Add result = new();
        var factorCopy = Factors.Cast<Real>().ToList();

        for (int i = 0; i < factorCopy.Count; i++)
        {
            var backup = factorCopy[i];
            var der = factorCopy[i].Differentiate();
            factorCopy[i] = der;
            result.AddOperand(
                new Multiply(factorCopy)
                );
            factorCopy[i] = backup;
        }
        result.Seal();
        return result;
    }

    public override Real Reverse(Real factor, Real target)
    {
        var allbutFactor = Factors.Exclude(Enumerable.Repeat(factor, 1)).Cast<Real>();

        var oneOverAll = allbutFactor.Select(n => Divide.Create(new Number(1), n));
        return new Multiply(oneOverAll.Append(target));
    }

    public Multiply() : base() { }
    public Multiply(IEnumerable<Real> words) : base(words) { }
    public Multiply(params Real[] words) : base(words) { }

    private static double rate(Real target)
    {
        if (target is Differential)
        {
            return 2;
        }
        else if (target is Proxy)
        {
            return 1;
        }
        else return 0;
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
        _neg = Factors.Any(n => n is Number { IsNegative: true } || n is Multiply { IsNegative: true });
        _co = Factors.OfType<Number>().Select(n => n.Value).Aggregate(new Complex(1),(start,current) => start * current);
        base.Seal();
    }
}
#region Simplifications
file class MultiplyNumberNumber : IRealSimplification<Number, Number, Multiply>
{
    public Real? Simplify(Number left, Number right)
    {
        return new Number(left.Value * right.Value);
    }
}
file class MultiplyWordWord : IRealSimplification<Real, Real, Multiply>
{
    public Real? Simplify(Real left, Real right)
    {
        if (!Equator.Equals(left, right)) return null;

        var add = new Add();
        add.AddOperand(new Number(2));
        return new Power(left, add);

    }
}

file class MultiplyNumberWord : IRealSimplification<Number, Real, Multiply>
{
    public Real? Simplify(Number left, Real right)
    {
        if (left.Value == 1) return right;
        else if (left.Value == 0) return left;
        else return null;
    }
}
file class MultiplyPowerWord : IRealSimplification<Power, Real, Multiply>
{
    public Real? Simplify(Power left, Real right)
    {
        if (!Equator.Equals(left.Base, right)) return null;

        if (left.Exponent is Add { IsSealed: false } add)
        {
            add.AddOperand(new Number(1));
            return left;
        }
        else return new Power(left.Base,
            new Add(left.Exponent, new Number(1)));
    }
}
file class MultiplyMultiplyWord : IRealSimplification<Multiply, Real, Multiply>
{
    public Real? Simplify(Multiply left, Real right)
    {
        if (!left.IsSealed)
        {
            left.AddOperand(right);
            return left;
        }
        else return new Multiply(left.Factors.Cast<Real>().Append(right));
    }
}
file class MultiplyPowerPower : IRealSimplification<Power, Power, Multiply>
{
    public Real? Simplify(Power left, Power right)
    {
        // x^a * x^b = x^(a+b)
        if (Equator.Equals(left.Base, right.Base))
        {
            return new Power(left.Base, new Add(left.Exponent, right.Exponent));
        }
        // x^a * y^a = (x*y) ^ a
        else if (Equator.Equals(left.Exponent, right.Exponent))
        {

            return new Power(
                new Multiply(left.Base, right.Base),
                left.Exponent
                );
        }
        return null;
    }
}
#endregion
