using SharpCalc.DataModels;
using SharpCalc.Exceptions;
using SharpCalc.Operators;
using SharpCalc.Operators.Arithmetic;
using System.Buffers;
using System.Diagnostics;
using System.Text;

namespace SharpCalc.Components;
/// <summary>
/// Wrapper class for C# functions whose input and output is <see cref="Complex"/>
/// </summary>
internal class NativeFunction : IFunction
{
    delegate Complex Wrapper(ReadOnlySpan<Complex> data);
 
    Wrapper wrapperFunc;
    
    static readonly char[] ParamNames = { 'x', 'y', 'z', 'a', 'b', 'c', 'd' };
    public int ParameterCount { get; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public IReadOnlyList<string> ParameterDescriptions { get; init; } = Array.Empty<string>();
    bool singular = false;
    IFunction? derivative;
    IFunction? inverse;
    public string Print()
    {
        StringBuilder paramdesc = new();
        for (int i = 0; i < ParameterDescriptions.Count; i++)
        {
            paramdesc.Append('\n');
            paramdesc.Append(ParamNames[0]);
            paramdesc.Append(": ");
            paramdesc.AppendLine(ParameterDescriptions[0]);
        }
        var paramList = string.Join(", ", ParamNames.Take(ParameterCount));
        return $"{Name}({paramList}) : {Description}{paramdesc}";
    }
    public Scalar? Run(ReadOnlySpan<IMathNode> paramlist)
    {        
        try
        {
            Span<Complex> vals = stackalloc Complex[ParameterCount];
            for (int index = 0; index < paramlist.Length; index++)
            {
                var item = paramlist[index];
                if (item.TryConvertToNumber(out Complex num))
                {
                    vals[index] = num;
                }
                else return null;
            }
            return new Number(wrapperFunc(vals));
        }
        catch (ApplicationException) { return null; }
    }

    public Scalar Differentiate(ReadOnlySpan<IMathNode> parameters)
    {
        if (derivative == null) throw new Exception($"the derivative for the function '{Name}' has not been declared");
        var call = derivative.Run(parameters) ?? new FunctionCall(derivative, parameters);
        return singular ? new Multiply([call, (parameters[0] as Scalar)!.Differentiate()]) : call;
    }
    public void SetDerivative(IFunction derivative, bool singular = true)
    {
        if (derivative.ParameterCount != ParameterCount) throw new Exception();
        if (this.derivative != null) throw new Exception();
        this.singular = singular;
        this.derivative = derivative;
    }
    public IFunction GetDerivative()
    {
        Debug.Assert(ParameterCount == 1 && derivative != null);
        return derivative;
    }

    public void SetInverse(IFunction inverse)
    {
        if (this.inverse != null) throw new Exception();
        this.inverse = inverse;
    }

    public Scalar Reverse(Scalar factor, Scalar target, ReadOnlySpan<IMathNode> paramlist)
    {
        if (inverse == null) throw new Exception($"the inverse for the function '{Name}' has not been declared");
        return new FunctionCall(inverse, target);
    }

    public Complex RunFast(ReadOnlySpan<Complex> paramlist) => wrapperFunc(paramlist);
    public bool Equals(IMathNode? other) => ReferenceEquals(this, other);
    
    public NativeFunction(Func<Complex> method) {
        wrapperFunc = (data) => method();
        ParameterCount = 0;
    }
    public NativeFunction(Func<Complex, Complex> method){
        wrapperFunc = (data) => method(data[0]);
        ParameterCount = 1;
    }
    public NativeFunction(Func<Complex, Complex, Complex> method)  {
        wrapperFunc = (data) => method(data[0], data[1]);
        ParameterCount = 2;
    }
    public NativeFunction(Func<Complex, Complex, Complex, Complex> method)  {
        wrapperFunc = (data) => method(data[0], data[1], data[2]);
        ParameterCount = 3;
    }
    public NativeFunction(Func<Complex, Complex, Complex, Complex, Complex> method) {
        wrapperFunc = (data) => method(data[0], data[1], data[2], data[3]);
        ParameterCount = 4;
    }
}
