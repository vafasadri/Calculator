using SharpCalc.Components;

namespace SharpCalc.DataModels
{
    /// <summary>
    /// Wrapper class for <see cref="Double"/> that implements the <seealso cref="IMathNode"/> interface
    /// </summary>
    internal readonly struct Number : Real
    {
        // that's it i'm doing this
        // i wanna add complex numbers to this thing
        public Complex Value { get; }     
        public bool IsNegative
        {
            get{
                return Value.IsReal() && Value.b < 0;
            }
        }
        public bool IsComplex
        {
            get
            {
                return Value.a != 0;
            }
        }
        string IMathNode.TypeName => "Number";
        public string ToText()
        {
            return Value.ToString();
        }
        Real? Real.Simplify()
        {
            return null;
        }           
        Real Real.Differentiate()
        {
            return new Number(0);
        }

        void Real.EnumerateVariables(ISet<Proxy> variables)
        {          
        }

        bool Real.ContainsVariable(Proxy variable) => false;   

        public static implicit operator Number(double value)
        {
            return new Number(value);
        }
        public Number(Complex value)
        {
            Value = value;
        }
        public Number(double value)
        {
            Value = new Complex( value);
        }

    }
}
