using SharpCalc.Components;
using SharpCalc.Operators.Arithmetic;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SharpCalc.DataModels
{
    public interface INamed
    {
        string Name { get; }
    }
    public interface IValued
    {
        Real? Value { get; }
    }
    /// <summary>
    /// Works just like <see cref="object"/> inside this calculator.
    /// <para> numbers,variables,functions and etc are all considered words</para>
    /// </summary>
    public interface IMathNode
    {
        /// <summary>
        /// Name of the deriving type, mostly used in exceptions like: 
        /// <code>exception: function expected, number provided</code>
        /// </summary>
        string TypeName { get; }
        /// <summary>
        /// when overridden in a deriving class, converts the word to a simple representation like its name        
        /// </summary>     
        string ToText();
        // Used to print the word with details
        virtual string Print()
        {
            return ToText();
        }
        // used to solve equations       
    }
    public interface Real : IMathNode
    {
        internal Real Differentiate();
        
        /// <summary>
        /// when overridden in a deriving class adds all the variables inside the word into the provided set
        /// </summary>        
        internal void EnumerateVariables(ISet<Proxy> variables);
        internal bool ContainsVariable(Proxy variable);

        /// <summary>
        /// when overridden in a deriving class, tries to simplify the word
        /// and returns the simplified version.
        /// <para>returns null in case no simplification is possible</para> 
        /// <para>example: 2 + 2 is simplified into 4</para> 
        /// </summary>
        internal Real? Simplify();

        public static Real operator*(Real left,Real right )
        {
            return new Multiply(left, right);
        }
        public static Real operator+(Real left,Real right)
        {
            return new Add(left, right);
        }
        public static Real operator -(Real left)
        {
            return Negative.Create(left);
        }
        public static Real operator ^(Real left,Real right)
        {
            return new Power(left, right);
        }
        public static Real operator /(Real left,Real right)
        {
            return Divide.Create(left,right);
        }
    }
}
