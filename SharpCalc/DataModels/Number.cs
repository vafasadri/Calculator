using SharpCalc.Components;

namespace SharpCalc.DataModels
{
    /// <summary>
    /// Wrapper class for <see cref="Double"/> that implements the <seealso cref="IMathNode"/> interface
    /// </summary>
    public readonly struct Number : Scalar
    {
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
        public string Render()
        {
            return Value.ToString();
        }
        IMathNode? IMathNode.SimplifyInternal()
        {
            return null;
        }           
        Scalar Scalar.Differentiate()
        {
            return new Number(0);
        }

        void Scalar.EnumerateVariables(ISet<Variable> variables)
        {          
        }

        bool Scalar.ContainsVariable(Variable variable) => false;
        public Complex ComputeNumerically() => Value;
        public bool Equals(IMathNode? other) => other is Number n && n.Value == Value;

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
