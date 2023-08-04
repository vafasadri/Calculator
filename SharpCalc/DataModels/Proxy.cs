using SharpCalc.DataModels;
namespace SharpCalc;
/// <summary>
/// a reference to a value with a name, mostly used as parameters in functions
/// <para>Variables are also considered Proxies</para>
/// <para/>example: in the function <code> f(x) = 2 * x </code>, x is a proxy which doesn't contain any value when declaring the function
/// and when you call the function f with <code>f(2)</code>, the value of x is set to 2
/// <para>example 2: in the method <seealso cref="EquationList.QuadraticEquation"/> there are 3 proxies in <seealso cref="EquationList.QuadraticParameters"/>, A, B and c which are assigned values once you call the method </para>
/// <para>example 3: once you mention a variable like X inside your math expressions; a proxy called x is created inside the data bank</para>
/// </summary>
internal class Proxy : IDataModel, IContent, IValued
{
    public virtual Word? Value { get; set; }
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
    Word? Word.Simplify()
    {
        return Value;
    }

    public virtual void FindX(VariableLocator locator)
    {
       Value?.FindX(locator);
    }

   public virtual Word Derivative(Proxy x)
    {
        if (Equator.Equals(this, x)) return new Number(1);
        else return new Number(0);
    }

    public Proxy(string name)
    {
        Name = name;
    }
}