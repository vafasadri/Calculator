using SharpCalc.DataModels;
using SharpCalc.Exceptions;
using SharpCalc.Operators.Arithmetic;

namespace SharpCalc
{
    internal class SigmaSummation : IFunction
    {
        public int ParameterCount => 3;
        public string Name => "sigma";
        public static IFunction Instance = new SigmaSummation();
        public Word Run(IReadOnlyList<Word> paramlist)
        {
            if (paramlist[0] is not IFunction) throw new ExpectingError("Function", "at first parameter in function call to 'sigma'", paramlist[0]);
            var func = (IFunction)paramlist[0];
            if (func.ParameterCount != 1) throw new Exception($"function {func.Name} takes {func.ParameterCount} parameter(s) and cannot be used in sigma summations");
            if (!Utilities.TryConvertToNumber(paramlist[1], out double min)) throw new ExpectingError("Number", "at second parameter in function call to 'sigma'", paramlist[1]);
            if (!Utilities.TryConvertToNumber(paramlist[2], out double max)) throw new ExpectingError("Number", "at third parameter in function call to 'sigma'", paramlist[2]);
            Add result = new();
            var words = new Word[] { null };          
            for (double i = min; i <= max; i++)
            {
                words[0] = new Number(i);
                var w = func.Run(words).SuperSimplify(out _);
                result.AddOperand(w);
            }
            return result;
            
        }
        void Word.FindX(VariableLocator locator)
        {
        }

        public Word? TryRun(IReadOnlyList<Word> paramlist)
        {

            return Run(paramlist);


        }

        private SigmaSummation()
        {

        }
    }
}
