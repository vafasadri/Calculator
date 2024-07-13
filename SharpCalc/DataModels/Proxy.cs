using SharpCalc.Components;
using SharpCalc.DataModels;

namespace SharpCalc;
/// <summary>
/// a reference to a value with a name, mostly used as parameters in functions
/// <para>Variables are also considered Proxies</para>
/// <para/>example: in the function <code> f(x) = 2 * x </code>, x is a proxy with a value of null when declaring the function
/// and when you call the function by writing <code>f(2)</code> the value of x is set to 2
/// <para>example 2: in the method <seealso cref="EquationList.QuadraticEquation"/> there are 3 proxies in <seealso cref="EquationList.QuadraticParameters"/>, A, B and c which are assigned values once you call the method </para>
/// <para>example 3: once you mention a variable like X inside your math expressions; a proxy called x is created inside the data bank</para>
/// </summary>
internal class Proxy : IDataModel, Real, IValued
{
    public virtual Real? Value { get; set; }
    public virtual Differential Differential { get; set; }
    public virtual bool HasValue => Value != null;
    public string Name { get; }
    public virtual string TypeName => "Value Proxy";
    public string ToText()
    {
        return Name;
    }
    public string Print()
    {
        return $"{Name} =>  {Value?.ToText() ?? "?"}";
    }
    
    /// <summary>
    /// a proxy is simplified into its value
    /// </summary>
    Real? Real.Simplify()
    {
        return Value;
    }

   public virtual Real Differentiate()
    {       
        return Differential;
        //if (Equator.Equals(this, x)) return new Number(1);
        //else return new Number(0);
    }

    public void EnumerateVariables(ISet<Proxy> variables)
    {
        variables.Add(this);
        Value?.EnumerateVariables(variables);
    }
    
    bool Real.ContainsVariable(Proxy variable) => Equator.Equals(this,variable);

    public Proxy(string name)
    {
        Name = name;
    }
}