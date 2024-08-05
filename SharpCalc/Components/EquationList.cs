using SharpCalc.DataModels;
using SharpCalc.Operators;
using SharpCalc.Operators.Arithmetic;

namespace SharpCalc.Components
{
    internal static class EquationList
    {            
        private static IFunction Quadratic1 = new RuntimeFunction("quadraticA", ["a", "b", "c"], "(-b + sqrt(b^2 - 4.a.c))/(2.a)");
        private static IFunction Quadratic2 = new RuntimeFunction("quadraticB", ["a", "b", "c"], "(-b - sqrt(b^2 - 4.a.c))/(2.a)");

        public static CandidateList QuadraticEquation(Scalar a, Scalar b, Scalar c)
        {
            return new CandidateList([Quadratic1.Run([a, b, c]), Quadratic2.Run([a,b,c])]);   
        }
    }
}
