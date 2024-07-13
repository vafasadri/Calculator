using SharpCalc.Components;
using SharpCalc.Exceptions;
using SharpCalc.Operators;
using SharpCalc.Operators.Arithmetic;
using System.Text;

namespace SharpCalc.DataModels;
/// <summary>
/// wrapper object for c# functions whose input and output is <see cref="double"/>
/// </summary>
internal class CompiledFunction : IFunction
{
    
    Delegate method;
    public string Name { get; private set; }
    static char[] ParamNames = { 'x', 'y', 'z', 'a', 'b', 'c', 'd' };
    public int ParameterCount { get; private set; }
    public string Description { get; private set; }
    public string[] paramDescriptions;
    Real derivative;
    Real inverse;
    public Proxy InverseParameter { get; private set; } = new("x");
    public Proxy DerivativeParameter { get; private set; } = new("x");
    public string Print()
    {
        StringBuilder paramdesc = new();
        for (int i = 0; i < paramDescriptions.Length; i++)
        {
            paramdesc.Append('\n');
            paramdesc.Append(ParamNames[0]);
            paramdesc.Append(": ");
            paramdesc.AppendLine(paramDescriptions[0]);
        }
        var paramList = string.Join(", ", ParamNames.Take(ParameterCount));
        return $"{Name}({paramList}) : {Description}{paramdesc}";
    }
    public Real? TryRun(IReadOnlyList<IMathNode> paramlist)
    {
        try
        {
            return Run(paramlist);
        }
        catch(InsufficientParametersError ex) { throw ex; }
        catch(ApplicationException) { return null; }      
    }
    public Real Run(IReadOnlyList<IMathNode> paramlist)
    {
        if (paramlist.Count != ParameterCount) throw new InsufficientParametersError(ParameterCount, paramlist.Count);
        object[] vals = new object[ParameterCount];
        for (int index = 0; index < paramlist.Count; index++)
        {
            var item = paramlist[index];
            if (item.TryConvertToNumber(out Complex num))
            {
                vals[index] = num;
            }
            else throw new ExpectingError("number", $"in function call to '{Name}'", item);
        }
        return new Number((Complex)method.DynamicInvoke(args: vals)!);
    }

    public Real Differentiate(Real parameter)
    {
        if (derivative == null) throw new Exception($"the derivative for the function '{Name}' has not been declared");
        var dev = parameter.Differentiate();
        DerivativeParameter.Value = parameter;
        var call = derivative.SuperSimplify(out _);
        DerivativeParameter.Value = null;
        return new Multiply(dev, call); 
    }
    public void SetDerivative(Real derivative)
    {
        if (this.derivative != null) throw new Exception();
        this.derivative = derivative;
    }
    public void SetInverse(Real derivative)
    {
        if (this.inverse != null) throw new Exception();
        this.inverse = derivative;
    }

    public Real Reverse(Real factor, Real target, IReadOnlyList<IMathNode> paramlist)
    {
        if (inverse == null) throw new Exception($"the inverse for the function '{Name}' has not been declared");
        InverseParameter.Value = target;

        var call = inverse.SuperSimplify(out _);
        InverseParameter.Value = null;
        return call;
    }

    private CompiledFunction(Delegate method, string name, string description, params string[] paramdescription)
    {
        Name = name;
        paramDescriptions = paramdescription;
        Description = description;
        this.method = method;
        ParameterCount = method.Method.GetParameters().Length;
    }

    public CompiledFunction(Func<Complex> method, string name, string description, params string[] paramdescription) : this(method as Delegate, name, description, paramdescription) { }
    public CompiledFunction(Func<Complex, Complex> method, string name, string description, params string[] paramdescription) : this(method as Delegate, name, description, paramdescription) { }
    public CompiledFunction(Func<Complex, Complex, Complex> method, string name, string description, params string[] paramdescription) : this(method as Delegate, name, description, paramdescription) { }
    public CompiledFunction(Func<Complex, Complex, Complex, Complex> method, string name, string description, params string[] paramdescription) : this(method as Delegate, name, description, paramdescription) { }
    public CompiledFunction(Func<Complex, Complex, Complex, Complex, Complex> method, string name, string description, params string[] paramdescription) : this(method as Delegate, name, description, paramdescription) { }
}
