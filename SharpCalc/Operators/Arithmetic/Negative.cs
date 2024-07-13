using SharpCalc.DataModels;
using System.Linq.Expressions;

namespace SharpCalc.Operators.Arithmetic
{
    static class Negative
    {
        public static Multiply Create(Real word)
        {
            return new Multiply(new Number(-1), word);
        }
    }
}
