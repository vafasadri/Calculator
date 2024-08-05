using SharpCalc.Components;
using SharpCalc.DataModels;
using SharpCalc.Exceptions;
using SharpCalc.Operators;

namespace SharpCalc.Functions
{
    internal class SigmaSummation : IFunction
    {
        public int ParameterCount => -1;
        public string Name => "sigma";
        public readonly static IFunction Instance = new SigmaSummation();
        public Scalar? Run(ReadOnlySpan<IMathNode> paramlist)
        {
            if (paramlist[0] is not IFunction) throw new ExpectingError("Function", "at first parameter in function call to 'sigma'", paramlist[0]);
            var func = (IFunction)paramlist[0];
            if (func.ParameterCount != 1) throw new SigmaInvalidFunctionException(func);
            if (!paramlist[1].TryConvertToNumber(out Complex min)) return null;
            if (!paramlist[2].TryConvertToNumber(out Complex max)) return null;
            double step = 1;
            if (paramlist.Length == 4 && paramlist[3].TryConvertToNumber(out Complex stepComplex))
            {
                if (!stepComplex.IsReal() || stepComplex.b == 0) throw new Exceptions.CustomError("Invalid value for step,step must be a real and positive value");
                step = stepComplex.b;
            }
            if (min > max) throw new Exceptions.CustomError("the upper bound cannot be less than the lower bound");

            Span<Complex> parameter = stackalloc Complex[1];
            ReadOnlySpan<Complex> readOnly = parameter;
            Complex sum = 0;
            for (double i = min.b; i <= max.b; i += step)
            {
                parameter[0] = i;
                var w = func.RunFast(readOnly);
                sum += w;
            }
            return new Number(sum * step);
        }
        public Scalar? TryRun(ReadOnlySpan<IMathNode> paramlist)
        {
            return Run(paramlist);
        }

        public Scalar Differentiate(ReadOnlySpan<IMathNode> parameter)
        {
            var func = (IFunction)parameter[0];
            var newParams = parameter.ToArray();
            newParams[0] = func.GetDerivative();
            return new FunctionCall(Instance, (ReadOnlySpan<IMathNode>)newParams);
        }
        public Scalar Reverse(Scalar factor, Scalar target, ReadOnlySpan<IMathNode> paramlist) => throw new NotImplementedException();
        public IFunction GetDerivative() => throw new NotImplementedException();
        public Complex RunFast(ReadOnlySpan<Complex> paramlist) => throw new NotImplementedException();
        public bool Equals(IMathNode? other) => throw new NotImplementedException();
        string Print()
        {
            return "sigma(f : function,a,b,step = 1) => f(a) + f(a + step) + ... + f(b)";

        }
        private SigmaSummation()
        {

        }
    }
}
