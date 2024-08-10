using SharpCalc.Components;
using SharpCalc.DataModels;
using System.Buffers;
using System.Text;
namespace SharpCalc.Operators.Arithmetic;
internal class Multiply : ScalarOperatorGroup
{
    public static readonly new OperatorGroupMetadata Metadata = new(typeof(Multiply),
        [new MultiplyNumberNumber(),
           new MultiplyNumberAny(),
           new MultiplyPowerPower(),
           new MultiplyPowerAny(),
           new MultiplyAnyAny(),
           new MultiplyAddAdd()])
    {
        Name = "Multiply",
        Precedence = 2,
        EmptyCreator = () => new Multiply(),
        FullCreator = (factors) => new Multiply(factors.Cast<Scalar>()),
        EmptyValue = new Number(1),
        UnpackSelfType = true
    };
    bool _neg;
    Complex _co;
    public bool IsNegative
    {
        get
        {
            if (!IsSealed) throw new Exceptions.CustomError("operator is still volatile");
            return _neg;
        }
    }
    public Complex Coefficient
    {
        get
        {
            if (!IsSealed) throw new Exceptions.CustomError("operator is still volatile");
            return _co;
        }
    }

    public override string Render()
    {
        var numerator = ArrayPool<string>.Shared.Rent(Factors.Count);
        var denominator = ArrayPool<string>.Shared.Rent(Factors.Count);
        int numeratorIndex = 0;
        int denominatorIndex = 0;
        foreach (var item in Factors)
        {
            if (item is Number) continue;
            else if (item is Power pow && (pow.Exponent is Number { IsNegative: true } or Multiply { IsNegative: true }))
            {
                var power = new Power(pow.Base, Negative.Create(pow.Exponent));
                denominator[denominatorIndex++] = WrapMember((power as Scalar).Simplify(out _));
            }
            else
            {
                numerator[numeratorIndex++] = WrapMember(item);
            }
        }
        Complex numberfactor = Coefficient;
        StringBuilder builder = new();
        if (numberfactor == -1) builder.Append('-');
        else if (numberfactor != 1 || numeratorIndex == 0)
        {
            var factorString = numberfactor.ToString();
            
            var wrap = factorString.Contains('+') || factorString.Contains(' ') || (char.IsLetter(factorString.Last()) && (numeratorIndex != 0 && char.IsLetter(numerator[0][0]))) || (char.IsDigit(factorString.Last()) && (numeratorIndex != 0 && char.IsDigit(numerator[0][0])));
            if (wrap) builder.Append('(');
            builder.Append(factorString);
            if (wrap) builder.Append(')');
        }
        builder.AppendJoin(" * ", numerator.Take(numeratorIndex));
        if (denominatorIndex != 0)
        {
            builder.Append(" / ");
            if (denominatorIndex > 1) builder.Append('(');
            builder.AppendJoin(" * ", denominator.Take(denominatorIndex));
            if (denominatorIndex > 1) builder.Append(')');
        }
        ArrayPool<string>.Shared.Return(numerator);
        ArrayPool<string>.Shared.Return(denominator);
        return builder.ToString();
    }
    public override IMathNode Convert(IMathNode value, Symbol symbol)
    {
        if (symbol == Symbol.Cross || symbol == Symbol.Null || symbol == Symbol.Point) return value;
        else if (symbol == Symbol.Slash) return new Power((Scalar)value, new Number(-1));
        else throw new Exception();
    }
    public override Scalar Differentiate()
    {
        Add result = new();

        var buffer = ArrayPool<Scalar>.Shared.Rent(Factors.Count);
        Memory<Scalar> memory = new(buffer, 0, Factors.Count);
        var factorCopy = memory.Span;
        for (int i = 0; i < factorCopy.Length; i++)
        {
            factorCopy[i] = (Scalar)Factors[i];
        }
        for (int i = 0; i < factorCopy.Length; i++)
        {
            var backup = factorCopy[i];
            var der = factorCopy[i].Differentiate();
            factorCopy[i] = der;
            result.AddOperand(new Multiply(buffer.Take(factorCopy.Length)));

            factorCopy[i] = backup;
        }
        result.Seal();
        ArrayPool<Scalar>.Shared.Return(buffer, true);
        return result;
    }

    public override Scalar Reverse(Scalar factor, Scalar target)
    {
        var allbutFactor = Factors.Exclude([factor], Equator.Instance).Cast<Scalar>();

        var oneOverAll = from n in allbutFactor select new Power(n, new Number(-1));
        return new Multiply(oneOverAll.Append(target));
    }

    public Multiply() : base() { }
    public Multiply(IEnumerable<Scalar> factors) : base(factors) { }
    private static double rate(Scalar target)
    {
        if (target is Differential)
        {
            return 2;
        }
        else if (target is Variable)
        {
            return 1;
        }
        else return 0;
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
        _neg = Factors.Any(n => n is Number { IsNegative: true } || n is Multiply { IsNegative: true });
        _co = Factors.OfType<Number>().Select(n => n.Value).Aggregate(new Complex(1), (start, current) => start * current);
    }

    public override Complex ComputeNumerically()
    {
        Complex result = new(1);
        foreach (var item in Factors)
        {
            result *= ((Scalar)item).ComputeNumerically();
        }
        return result;
    }

    public override bool Equals(IMathNode? other)
    {
        return false;
        throw new NotImplementedException();
    }
}
#region Simplifications
file class MultiplyNumberNumber : ISimplification<Number, Number, Multiply>
{
    public IMathNode? Simplify(Number left, Number right)
    {
        return new Number(left.Value * right.Value);
    }
}
file class MultiplyAnyAny : ISimplification<Scalar, Scalar, Multiply>
{
    public IMathNode? Simplify(Scalar left, Scalar right)
    {
        if (!Equator.Equals(left, right)) return null;
        
        return new Power(left, new Number(2));
    }
}

file class MultiplyNumberAny : ISimplification<Number, Scalar, Multiply>
{
    public IMathNode? Simplify(Number left, Scalar right)
    {
        if (left.Value == 1) return right;
        else if (left.Value == 0) return left;
        else return null;
    }
}
file class MultiplyPowerAny : ISimplification<Power, Scalar, Multiply>
{
    public IMathNode? Simplify(Power left, Scalar right)
    {
        if (!Equator.Equals(left.Base, right)) return null;
        
        return new Power(left.Base,
            new Add([left.Exponent, new Number(1)]));
    }
}
file class MultiplyPowerPower : ISimplification<Power, Power, Multiply>
{
    public IMathNode? Simplify(Power left, Power right)
    {
        // x^a * x^b = x^(a+b)
        if (Equator.Equals(left.Base, right.Base))
        {
            return new Power(left.Base, new Add([left.Exponent, right.Exponent]));
        }
        return null;
    }
}
file class MultiplyAddAdd : ISimplification<Add, Add, Multiply>
{
    public IMathNode? Simplify(Add left, Add right)
    {
        var add = new Add();
        foreach (var i in left.Factors)
        {
            foreach (var j in right.Factors)
            {
                add.AddOperand(new Multiply([(Scalar)i, (Scalar)j]));
            }
        }
        add.Seal();
        return add;
    }
}
#endregion
