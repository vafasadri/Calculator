using SharpCalc.DataModels;
using SharpCalc.Operators;
using SharpCalc.Operators.Arithmetic;

namespace SharpCalc.Components
{
    public static class EquationList
    {
        private static class QuadraticParameters
        {
            public static readonly Proxy a = new("a");
            public static readonly Proxy b = new("b");
            public static readonly Proxy c = new("c");
        }

        private static readonly Real Delta =
             new Add(new Power(QuadraticParameters.b, new Number(2)) // b^2
                 , new Multiply(new Number(-4), QuadraticParameters.a, QuadraticParameters.c)); //  - 4.a.c
        private static Real QuadraticFormula = new Multiply(new Add(Negative.Create(QuadraticParameters.b), new FunctionCall(StaticDataBank.sqrt, Delta)), new Number(1 / 2), new Power(QuadraticParameters.a, new Number(-1))); // -b + sqrt(delta) / 2a
        public static Real QuadraticEquation(Real a, Real b, Real c)
        {
            lock (QuadraticParameters.a)
                lock (QuadraticParameters.b)
                    lock (QuadraticParameters.c)
                    {
                        QuadraticParameters.a.Value = a;
                        QuadraticParameters.b.Value = b;
                        QuadraticParameters.c.Value = c;
                        var output = QuadraticFormula.SuperSimplify(out _);
                        QuadraticParameters.a.Value = null;
                        QuadraticParameters.b.Value = null;
                        QuadraticParameters.c.Value = null;
                        return output;
                    }
        }
    }
}
