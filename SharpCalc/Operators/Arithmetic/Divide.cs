using SharpCalc.DataModels;

namespace SharpCalc.Operators.Arithmetic
{
    static class Divide
    {
        public static Multiply Create(Word a, Word b)
        {
            return new Multiply(a, new Power(b, new Number(-1)));
        }
    }
}
