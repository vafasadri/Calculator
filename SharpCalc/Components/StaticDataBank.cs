using SharpCalc.DataModels;
using SharpCalc.Functions;
using System.Collections;

namespace SharpCalc.Components;

using CF = NativeFunction;
using param1 = Func<Complex, Complex>;
/// <summary>
/// a collection of common math functions and variables, like "e","pi","cos" and etc open for third party references
/// </summary>
public class StaticDataBank
{
    private class InternalDataBank : IReadonlyDataBank
    {
        internal static readonly List<IDataModel> _bank = new()
        {
            pi,e,ln,
            rand,remainder,floor,round,ceil,min,max,clamp,i,
            sin,tan,cos,cot,sinh,tanh,cosh,coth,arcsin,arctan,arccos,arccot,
            abs,conj,arg,re,im,sign,
            d,sigma,
            True,False
        };

        public bool ContainsName(string name)
        {
            return _bank.Any(n => n.Name == name);
        }
        public IDataModel? GetData(string name)
        {
            return _bank.Find(n => n != null && n.Name == name);
        }
        public IEnumerator<IDataModel> GetEnumerator()
        {
            return _bank.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _bank.GetEnumerator();
        }
    }
    static private Func<Complex, Complex> Wrapper(Func<double, double> func)
    {
        return (a) =>
        {
            if (!a.IsReal()) return new Complex(double.NaN, double.NaN);
            return (Complex)func(a.b);
        };
    }
    static private Func<Complex, Complex, Complex> wrapper(Func<double, double, double> func)
    {
        return (a, b) =>
        {
            if (!a.IsReal() || !b.IsReal()) return new Complex(double.NaN, double.NaN);
            return (Complex)func(a.b, b.b);
        };
    }
    static private Func<Complex,Complex,Complex,Complex> wrapper(Func<double, double, double, double> func)
    {
        return (a, b, c) =>
        {
            if (!a.IsReal() || !b.IsReal() || !c.IsReal()) return new Complex(double.NaN, double.NaN);
            return (Complex) func(a.b, b.b, c.b);
        };
    }
    internal static readonly Constant i = new("i", ComplexMath.i);
    internal static readonly Constant pi = new("pi", Math.PI);
    internal static readonly Constant e = new("e", Math.E);

    internal static readonly CF sin = new(ComplexMath.Sin) { Name = "sin", Description = "Sine of x radians" };
    internal static readonly CF tan = new(ComplexMath.Tan) { Name = "tan", Description = "Tangent of x radians" };
    internal static readonly CF cos = new(ComplexMath.Cos) { Name = "cos", Description = "Cosine of x radians" };
    internal static readonly CF cot = new(ComplexMath.Cot) { Name = "cot", Description = "Cotangent of x radians" };

    internal static readonly CF arcsin = new(ComplexMath.Arcsin) { Name = "arcsin", Description = "Inverse Sine of x radians" };
    internal static readonly CF arccos = new(ComplexMath.Arccos) { Name = "arccos", Description = "Inverse Cosine of x radians" };
    internal static readonly CF arctan = new(ComplexMath.Arctan) { Name = "arctan", Description = "Inverse Tangent of x radians" };
    internal static readonly CF arccot = new(ComplexMath.Arccot) { Name = "arccot", Description = "Inverse Cotangent of x radians" };

    internal static readonly CF sinh = new(ComplexMath.Sinh) { Name = "sinh", Description = "Hyperbolic Sine of x radians" };
    internal static readonly CF tanh = new(ComplexMath.Tanh) { Name = "tanh", Description = "Hyperbolic Tangent of x radians" };
    internal static readonly CF cosh = new(ComplexMath.Cosh) { Name = "cosh", Description = "Hyperbolic Cosine of x radians" };
    internal static readonly CF coth = new(ComplexMath.Coth) { Name = "coth", Description = "Hyperbolic Cotangent of x radians" };
    internal static readonly CF ln = new((param1)ComplexMath.Log) { Name = "ln", Description = "natural logarithm of x (base e)" };

    internal static readonly CF abs = new((Complex x) => (Complex)ComplexMath.Abs(x)) { Name = "abs", Description = "The absolute value of x" };
    internal static readonly CF sign = new(ComplexMath.Sign) { Name = "sign", Description = "sign of x" };
    internal static readonly CF conj = new(ComplexMath.Conj) { Name = "conj", Description = "conjugate of x" };
    internal static readonly CF arg = new((Complex x) => (Complex)ComplexMath.Arg(x)) { Name = "arg", Description = "argument of x" };
    internal static readonly CF re = new((Complex x) => (Complex)ComplexMath.Re(x)) { Name = "Re", Description = "Real part of x" };
    internal static readonly CF im = new((Complex x) => (Complex)ComplexMath.Im(x)) { Name = "Im", Description = "Imaginary part of x" };
    internal static readonly CF rand = new(() => (Complex)System.Random.Shared.Next()) { Name = "random", Description = "a randomly generated integer" };

    internal static readonly CF remainder = new(wrapper(Math.IEEERemainder)) { Name = "remainder", Description = "the Remainder of x divided by y" };
    internal static readonly CF floor = new(ComplexMath.Floor) { Name = "floor", Description = "x rounded downwards" };
    internal static readonly CF round = new(ComplexMath.Round) { Name = "round", Description = "x rounded to the nearest integer" };
    internal static readonly CF ceil = new(ComplexMath.Ceiling) { Name = "ceil", Description = "x rounded upwards" };
    internal static readonly IFunction max = Max.Instance;
    internal static readonly IFunction min = Min.Instance;
    internal static readonly CF clamp = new(wrapper(Math.Clamp)) { Name = "clamp", Description = "no description" };
    internal static readonly IFunction d = Differentiate.Instance;
    internal static readonly IFunction sigma = SigmaSummation.Instance;
    public static IReadonlyDataBank DataBank { get; } = new InternalDataBank();
    internal static readonly RuntimeFunction sqrt;
    internal static readonly RuntimeFunction cbrt;
    internal static readonly RuntimeFunction nrt;
    internal static readonly RuntimeFunction deg;
    internal static readonly RuntimeFunction rad;
    internal static readonly FunctionOverload log;
    internal static readonly RuntimeFunction exp;
    public static IDataModel True => Bool.True;
    public static IDataModel False => Bool.False;
    public static IDataModel Pi => pi;
    public static IDataModel E => e;
    public static IDataModel Sine => sin;
    public static IDataModel Cosine => cos;
    public static IDataModel Tangent => tan;
    public static IDataModel Cotangent => cot;
    public static IDataModel Degree => deg;
    public static IDataModel Logarithm => log;
    public static IDataModel NaturalLogarithm => ln;
    public static IDataModel SquareRoot => sqrt;
    public static IDataModel CubicRoot => cbrt;
    public static IDataModel NthRoot => nrt;
    public static IDataModel Random => rand;
    public static IDataModel AbsoluteValue => abs;
    public static IDataModel Remainder => remainder;
    public static IDataModel Floor => floor;
    public static IDataModel Round => round;
    public static IDataModel Ceiling => ceil;
    public static IDataModel Minimum => min;
    public static IDataModel Maximum => max;
    public static IDataModel Clamp => clamp;

    static StaticDataBank()
    {
        sqrt = new RuntimeFunction("sqrt", ["x"], "x^0.5");
        cbrt = new RuntimeFunction("cbrt", ["x"], "x^(1/3)");
        nrt = new RuntimeFunction("nrt", ["x", "a"], "x^(1/a)");
        deg = new RuntimeFunction("deg", ["x"], "x * pi /180");
        rad = new RuntimeFunction("rad", ["x"], "180x / pi");
        log = new FunctionOverload("log",
            [
            new RuntimeFunction("log", ["x", "a"], "ln(x) / ln(a)"),
            new RuntimeFunction("log", ["x"], "ln(x) / ln(10)")
            ]);
        exp = new RuntimeFunction("exp", ["x"], "e^x");

        InternalDataBank._bank.AddRange([log, sqrt, cbrt, nrt,
            deg, rad,exp]);
        sin.SetDerivative(new RuntimeFunction("sin'", ["x"], "cos(x)"));
        cos.SetDerivative(new RuntimeFunction("cos'", ["x"], "-sin(x)"));
        tan.SetDerivative(new RuntimeFunction("tan'", ["x"], "cos(x)^(-2)"));
        cot.SetDerivative(new RuntimeFunction("cot'", ["x"], "sin(x)^(-2)"));
        arcsin.SetDerivative(new RuntimeFunction("arcsin'", ["x"], "(1-x^2)^(-0.5)"));
        arccos.SetDerivative(new RuntimeFunction("arccos'", ["x"], "-(1-x^2)^(-0.5)"));
        arctan.SetDerivative(new RuntimeFunction("arctan'", ["x"], "1/(1+x^2)"));
        arccot.SetDerivative(new RuntimeFunction("arccot'", ["x"], "-1/(1+x^2)"));

        sinh.SetDerivative(new RuntimeFunction("sinh'", ["x"], "cosh(x)"));
        cosh.SetDerivative(new RuntimeFunction("cosh'", ["x"], "sinh(x)"));

        ln.SetDerivative(new RuntimeFunction("ln'", ["x"], "1/x"));
        abs.SetDerivative(new RuntimeFunction("abs'", ["x"], "sign(x)"));
        sign.SetDerivative(new RuntimeFunction("sign'", ["x"], "0"));
        sin.SetInverse(arcsin);
        cos.SetInverse(arccos);
        tan.SetInverse(arctan);
        cot.SetInverse(arccot);
        ln.SetInverse(exp);
    }
}

