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
internal class Variable : Proxy, Scalar
{
    public virtual new Scalar? Value { get => (Scalar?)base.Value; set => base.Value = value; }

    protected Differential differentialCache;
    public virtual Differential Differential {
        get
        {
            differentialCache ??= new(this);
            return differentialCache;
        }
    }
    public override string TypeName => "Variable";
   
    
    /// <summary>
    /// a proxy is simplified into its value
    /// </summary>   
   public virtual Scalar Differentiate()
   {       
        return Differential;      
   }

    public void EnumerateVariables(ISet<Variable> variables)
    {
        variables.Add(this);
        Value?.EnumerateVariables(variables);
    }
    
    bool Scalar.ContainsVariable(Variable variable) => Equator.Equals(this,variable);
    public virtual Complex ComputeNumerically()
    {
        if (Value != null)
        {
            return Value.ComputeNumerically();
        }
        else throw new Exceptions.CustomError($"Variable with unknown value: {Name}");
    }
    
    public Variable(string name) : base(name)
    {      
    }
}