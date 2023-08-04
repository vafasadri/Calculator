using SharpCalc.Operators;
using System.Security.Principal;
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
        return $"{Name}({string.Join(", ", ParamNames.Take(ParameterCount))}) : {Description}{paramdesc}";
    }
    public Word Run(IReadOnlyList<Word> paramlist)
    {
        var tryRun = TryRun(paramlist);
        if (tryRun == null) throw new Exception("function call failed");
        return tryRun;
    }
    public Word? TryRun(IReadOnlyList<Word> paramlist)
    {
        if (paramlist.Count != ParameterCount) throw new Exceptions.InsufficientParametersError(ParameterCount, paramlist.Count);
        object[] vals = new object[ParameterCount];
        int index = 0;
        foreach (var item in paramlist)
        {          
            if (item.TryConvertToNumber(out double num))
            {
                vals[index] = num;
            }
            else return null;
            index++;
        }
        
           return new Number((double)method.DynamicInvoke(args: vals)!);      
       
    }

    void Word.FindX(VariableLocator variable)
    {       
    }

    private CompiledFunction(Delegate method, string name, string description, params string[] paramdescription)
    {
        Name = name;
        paramDescriptions = paramdescription;
        Description = description;
        this.method = method;
        ParameterCount = method.Method.GetParameters().Length;
    }
    public CompiledFunction(Func<double> method, string name, string description, params string[] paramdescription) : this(method as Delegate, name, description, paramdescription) { }
    public CompiledFunction(Func<double, double> method, string name, string description, params string[] paramdescription) : this(method as Delegate, name, description, paramdescription) { }
    public CompiledFunction(Func<double, double, double> method, string name, string description, params string[] paramdescription) : this(method as Delegate, name, description, paramdescription) { }
    public CompiledFunction(Func<double, double, double, double> method, string name, string description, params string[] paramdescription) : this(method as Delegate, name, description, paramdescription) { }
    public CompiledFunction(Func<double, double, double, double, double> method, string name, string description, params string[] paramdescription) : this(method as Delegate, name, description, paramdescription) { }
}
