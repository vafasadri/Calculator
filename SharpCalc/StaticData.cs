using SharpCalc.DataModels;
using SharpCalc.Operators.Arithmetic;
using System.Collections;

namespace SharpCalc;

using CF = CompiledFunction;
using param1 = Func<double, double>;
using param2 = Func<double, double, double>;
/// <summary>
/// a collection of common math functions and variables, like "e","pi","cos" and etc open for third party references
/// </summary>
public class StaticData
{
    private class StaticDataBank : IReadonlyDataBank
    {
        internal static readonly List<IDataModel> _bank = new()
        {
            pi,e,sin,tan,cos,cot,log,ln,sqrt,cbrt,nrt,
            rand,abs,remainder,floor,round,ceil,min,max,clamp,deg,SigmaSummation.Instance
        };

        public bool ContainsName(string name)
        {
            return _bank.Any(n => n.Name == name);
        }
        public IDataModel? GetData(string name)
        {
            return _bank.Find(n => n.Name == name);
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

    internal static readonly Constant pi = new("pi", Math.PI);
    internal static readonly Constant e = new("e", Math.E);
    internal static readonly CF sin = new(Math.Sin, "sin", "Sine of x radians");
    internal static readonly CF tan = new(Math.Tan, "tan", "Tangent of x radians");
    internal static readonly CF cos = new(Math.Cos, "cos", "Cosine of x radians");
    internal static readonly CF cot = new((double x) => 1 / Math.Tan(x), "cot", "Cotangent of x radians");
    //public static readonly CF Degree = new((double x) => x * Math.PI / 180, "deg", "Radian equivalent of x degrees");
    internal static readonly CF log = new((param2)Math.Log, "log", "logarithm of x by base y");
    internal static readonly CF ln = new((param1)Math.Log, "ln", "natural logarithm of x (base e)");
    internal static readonly CF sqrt = new(Math.Sqrt, "sqrt", "Square root of x");
    internal static readonly CF cbrt = new(Math.Cbrt, "cbrt", "Cubic root of x");
    internal static readonly CF nrt = new((double x, double y) => Math.Pow(x, 1 / y), "root", "y-th root of x");
    internal static readonly CF rand = new(() => System.Random.Shared.Next(), "rand", "a randomly generated integer");
    internal static readonly CF abs = new(Math.Abs, "abs", "Absolute Value", "The absolute value of x");
    internal static readonly CF remainder = new(Math.IEEERemainder, "remiander", "the Remainder of x divided by y");
    internal static readonly CF floor = new(Math.Floor, "floor", "x rounded downwards");
    internal static readonly CF round = new(Math.Round, "round", "x rounded to the nearest integer");
    internal static readonly CF ceil = new(Math.Ceiling, "ceil", "x rounded upwards");
    internal static readonly CF min = new(Math.Min, "min", "Minimum of x and y");
    internal static readonly CF max = new(Math.Max, "max", "Maximum of x and y");
    internal static readonly CF clamp = new(Math.Clamp, "clamp", "no description");
    internal static readonly RuntimeFunction deg;
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
    public static IReadonlyDataBank DataBank { get; } = new StaticDataBank();
    static StaticData()
    {
        var degreeParams = new RuntimeFunction.Parameter[] { new("x") };
        var mul = new Number(1.0 / 180.0);
        deg = new RuntimeFunction("deg", new Multiply(degreeParams[0], Pi, mul), degreeParams);
    }
}

