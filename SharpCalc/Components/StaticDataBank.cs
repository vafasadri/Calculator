using SharpCalc.DataModels;
using SharpCalc.Operators;
using SharpCalc.Operators.Arithmetic;
using System.Collections;
using System.Runtime.InteropServices;

namespace SharpCalc.Components;

using CF = CompiledFunction;
using param1 = Func<Complex, Complex>;
using param2 = Func<Complex, Complex, Complex>;
/// <summary>
/// a collection of common math functions and variables, like "e","pi","cos" and etc open for third party references
/// </summary>
public class StaticDataBank
{
    private class InternalDataBank : IReadonlyDataBank
    {
        internal static readonly List<IDataModel> _bank = new()
        {
            pi,e,log,ln,sqrt,cbrt,nrt,
            rand,remainder,floor,round,ceil,min,max,clamp,deg,rad,SigmaSummation.Instance,i,
            sin,tan,cos,cot,sinh,tanh,cosh,coth,arcsin,arctan,arccos,arccot,
            abs,conj,arg,re,im,sign,Differentiate.Instance
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
    static private Complex wrapper(Complex a, Func<double, double> func)
    {
        if (!a.IsReal()) return new Complex(double.NaN, double.NaN);
        return func(a.b);
    }
    static private Complex wrapper(Complex a,Complex b,Func<double,double,double> func)
    {
        if (!a.IsReal() || !b.IsReal()) return new Complex(double.NaN, double.NaN);
        return func(a.b, b.b);
    }
    static private Complex wrapper(Complex a, Complex b,Complex c, Func<double, double, double,double> func)
    {
        if (!a.IsReal() || !b.IsReal() || !c.IsReal()) return new Complex(double.NaN, double.NaN);
        return func(a.b, b.b,c.b);
    }
    internal static readonly Constant i = new("i", ComplexMath.i);
    internal static readonly Constant pi = new("pi",new Complex( Math.PI));
    internal static readonly Constant e = new("e",  new Complex(Math.E));

    internal static readonly CF sin = new(ComplexMath.Sin, "sin", "Sine of x radians");
    internal static readonly CF tan = new(ComplexMath.Tan, "tan", "Tangent of x radians");
    internal static readonly CF cos = new(ComplexMath.Cos, "cos", "Cosine of x radians");
    internal static readonly CF cot = new(ComplexMath.Cot, "cot", "Cotangent of x radians");
    
    internal static readonly CF arcsin = new(ComplexMath.Arcsin, "arcsin", "Inverse Sine of x radians");
    internal static readonly CF arccos = new(ComplexMath.Arccos, "arccos", "Inverse Cosine of x radians");
    internal static readonly CF arctan = new(ComplexMath.Arctan, "arctan", "Inverse Tangent of x radians");
    internal static readonly CF arccot = new(ComplexMath.Arccot, "arccot", "Inverse Cotangent of x radians");

    internal static readonly CF sinh = new(ComplexMath.Sinh, "sinh", "Hyperbolic Sine of x radians");
    internal static readonly CF tanh = new(ComplexMath.Tanh, "tanh", "Hyperbolic Tangent of x radians");
    internal static readonly CF cosh = new(ComplexMath.Cosh, "cosh", "Hyperbolic Cosine of x radians");
    internal static readonly CF coth = new(ComplexMath.Coth, "coth", "Hyperbolic Cotangent of x radians");
   
    
    internal static readonly CF ln = new((param1)ComplexMath.Log, "ln", "natural logarithm of x (base e)");

   
    internal static readonly CF abs = new((Complex x) => (Complex)ComplexMath.Abs(x), "abs", "Absolute Value", "The absolute value of x");
    internal static readonly CF sign = new(ComplexMath.Sign, "sign", "Absolute Value", "The absolute value of x");
    internal static readonly CF conj = new(ComplexMath.Conj, "conj", "Sine of x radians");
    internal static readonly CF arg = new((Complex x) => (Complex)ComplexMath.Arg(x), "arg", "Tangent of x radians");
    internal static readonly CF re = new((Complex x) => (Complex) ComplexMath.Re(x), "Re", "Cosine of x radians");
    internal static readonly CF im = new((Complex x) => (Complex) ComplexMath.Im(x), "Im", "Cotangent of x radians");

    
    internal static readonly RuntimeFunction sqrt;
    internal static readonly RuntimeFunction cbrt;
    internal static readonly RuntimeFunction nrt;
    internal static readonly CF rand = new(() => (Complex) System.Random.Shared.Next(), "rand", "a randomly generated integer");
    
    internal static readonly CF remainder = new((Complex x,Complex y) => wrapper(x,y,Math.IEEERemainder), "remiander", "the Remainder of x divided by y");
    internal static readonly CF floor = new((Complex x) =>  wrapper(x,Math.Floor), "floor", "x rounded downwards");
    internal static readonly CF round = new((Complex x) => wrapper(x,Math.Round), "round", "x rounded to the nearest integer");
    internal static readonly CF ceil = new((Complex x) => wrapper(x,Math.Ceiling), "ceil", "x rounded upwards");
    internal static readonly CF min = new((Complex x,Complex y) => wrapper(x,y,Math.Min), "min", "Minimum of x and y");
    internal static readonly CF max = new((Complex x,Complex y) => wrapper(x,y,Math.Max), "max", "Maximum of x and y");
    internal static readonly CF clamp = new((Complex x,Complex y,Complex z) => wrapper(x,y,z,Math.Clamp), "clamp", "no description");
    
    internal static readonly RuntimeFunction deg;
    internal static readonly RuntimeFunction rad;
    internal static readonly RuntimeFunction log;

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
    //public static IDataModel Remainder => remainder;
    //public static IDataModel Floor => floor;
    //public static IDataModel Round => round;
    //public static IDataModel Ceiling => ceil;
    //public static IDataModel Minimum => min;
    //public static IDataModel Maximum => max;
    //public static IDataModel Clamp => clamp;
    public static IReadonlyDataBank DataBank { get; } = new InternalDataBank();
    
        // do not use recursion on this   
    static StaticDataBank()
    {
        var degreeParams = new RuntimeFunction.Parameter[] { new("x") };
        var mul = new Number(1.0 / 180.0);
        deg = new RuntimeFunction("deg", new Multiply(degreeParams[0], pi, mul), degreeParams);
        var sqrtParams = new RuntimeFunction.Parameter[] { new("x") };
        sqrt = new RuntimeFunction("sqrt",new Power(sqrtParams[0],new Number(0.5)), sqrtParams);
        var cbrtParams = new RuntimeFunction.Parameter[] { new("x") };
        cbrt = new RuntimeFunction("cbrt", new Power(cbrtParams[0], new Number(1.0/3.0)), cbrtParams);
        var nrtParams = new RuntimeFunction.Parameter[] { new("x"),new("n") };
        nrt = new RuntimeFunction("root", new Power(nrtParams[0],Divide.Create(new Number(1) , nrtParams[1])), nrtParams);
        var radParams = new RuntimeFunction.Parameter[] { new("x") };
        rad = new RuntimeFunction("rad", new Multiply(radParams[0], new Number(180.0),new Power(pi,new Number(-1))),radParams);
        var logParams = new RuntimeFunction.Parameter[] { new("x"),new("y") };
        log = new RuntimeFunction("log",
            Divide.Create(new FunctionCall(ln, logParams[0]),new FunctionCall(ln, logParams[1]))
            , logParams);
        sin.SetDerivative(new FunctionCall( cos,sin.DerivativeParameter));
        cos.SetDerivative(Negative.Create(new FunctionCall(sin,cos.DerivativeParameter)));
        tan.SetDerivative(new Power(new FunctionCall(cos,tan.DerivativeParameter), new Number(-2)));
        cot.SetDerivative(new Power(new FunctionCall(sin, cot.DerivativeParameter), new Number(-2)));
        arcsin.SetDerivative(new Power(new Add(new Number(1),new Multiply(new Number(-1),arcsin.DerivativeParameter,arcsin.DerivativeParameter)) , new Number(-0.5)));
        arccos.SetDerivative(Negative.Create( new Power(new Add(new Number(1), new Multiply(new Number(-1), arcsin.DerivativeParameter, arcsin.DerivativeParameter)), new Number(-0.5))));
        arctan.SetDerivative(Divide.Create(new Number(1),new Add(new Number(1),new Power(arctan.DerivativeParameter,new Number(2)))));
        arccot.SetDerivative(Negative.Create( Divide.Create(new Number(1),new Add(new Number(1),new Power(arctan.DerivativeParameter,new Number(2))))));

        sinh.SetDerivative(new FunctionCall(cosh,sinh.DerivativeParameter));
        cosh.SetDerivative(new FunctionCall(sinh, cosh.DerivativeParameter));

        ln.SetDerivative(new Power(ln.DerivativeParameter, new Number(-1)));
        abs.SetDerivative(new FunctionCall(sign, abs.DerivativeParameter));
        sign.SetDerivative(new Number(0));

        sin.SetInverse(new FunctionCall(arcsin, sin.InverseParameter));
        cos.SetInverse(new FunctionCall(arccos, cos.InverseParameter));
        tan.SetInverse(new FunctionCall(arctan, tan.InverseParameter));
        cot.SetInverse(new FunctionCall(arccot, cot.InverseParameter));
        ln.SetInverse(new Power(e,ln.InverseParameter));       
        
    }
}

