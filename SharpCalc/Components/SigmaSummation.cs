using SharpCalc.DataModels;
using SharpCalc.Exceptions;
using SharpCalc.Operators.Arithmetic;

namespace SharpCalc.Components
{
    internal class SigmaSummation : IFunction
    {
        public int ParameterCount => 3;
        public string Name => "sigma";
        public readonly static IFunction Instance = new SigmaSummation();
        public Real Run(IReadOnlyList<IMathNode> paramlist)
        {
            if (paramlist[0] is not IFunction) throw new ExpectingError("Function", "at first parameter in function call to 'sigma'", paramlist[0]);
            var func = (IFunction)paramlist[0];
            if (func.ParameterCount != 1) throw new SigmaInvalidFunctionException(func);
            if (!paramlist[1].TryConvertToNumber(out Complex min)) throw new ExpectingError("Number", "at second parameter in function call to 'sigma'", paramlist[1]);
            if (!paramlist[2].TryConvertToNumber(out Complex max)) throw new ExpectingError("Number", "at third parameter in function call to 'sigma'", paramlist[2]);
            if (min.a != 0 || max.a != 0) throw new Exception("Cannot compare two complex numbers");
            Add result = new();
            var words = new Real[] { null };        
            for (double i = min.b; i <= max.b; i++)
            {
                words[0] = new Number(i);
                var w = func.Run(words).SuperSimplify(out _);
                result.AddOperand(w);
            }
            return result;
        }
        public Real? TryRun(IReadOnlyList<IMathNode> paramlist)
        {
            return Run(paramlist);
        }

        public Real Differentiate( Real parameter) => throw new NotImplementedException();
        public Real Reverse(Real factor, Real target, IReadOnlyList<IMathNode> paramlist) => throw new NotImplementedException();

        private SigmaSummation()
        {

        }
    }
}
