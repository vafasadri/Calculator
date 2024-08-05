using SharpCalc.Components;
using SharpCalc.Operators.Arithmetic;
using System.Diagnostics;

namespace SharpCalc.DataModels;

public interface INamed
{
    string Name { get; }
}  
/// <summary>
/// Works just like <see cref="object"/> inside this calculator.
/// <para> numbers,variables,functions and etc are all considered nodes</para>
/// </summary>
public interface IMathNode : IEquatable<IMathNode>
{
    /// <summary>
    /// Name of the deriving type, mostly used in exceptions like: 
    /// <code>exception: 'function' expected, 'number' provided</code>
    /// </summary>
    string TypeName { get; }
    /// <summary>
    /// when overridden in a deriving class, converts the node to a simple representation like its name        
    /// </summary>     
    string Render();
    /// <summary>
    /// Used to print the node in detail
    /// </summary>
    /// <returns></returns>
    string Print()
    {
        return Render();
    }
    /// <summary>
    /// simplifies the expression and returns the simplified version.
    /// <para>returns a referenece to the current instance in case no simplification is possible</para> 
    /// <para>example: 2 + 2 is simplified into 4</para>
    /// </summary>
    [DebuggerStepThrough]
    public IMathNode Simplify(out bool did)
    {
        did = false;
        IMathNode output = this;
        IMathNode? newOutput;
        while ((newOutput = output.SimplifyInternal()) != null)
        {
            did = true;
            output = newOutput;
        }
        return output;
    }
    protected IMathNode? SimplifyInternal();       
}

