using SharpCalc.DataModels;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

namespace SharpCalc.Operators
{
    internal class FunctionCall : IOperator
    {
        public static readonly SingleOperatorMetadata Metadata = SingleOperatorMetadata.Register
        (
           "Function Call", -1,
            (left, right) => new FunctionCall((IFunction)left, right)
        );
        internal readonly IFunction Function;
        internal readonly Word Parameter;
        OperatorMetadata IOperator.Metadata => Metadata;

        Word? Call(Word Parameter)
        {
            IReadOnlyList<Word> paramReady;        
           
            if (Parameter is Tuple parameters)
            {

                paramReady = parameters.Content;
            }
            else
            {
                paramReady = new Word[] { Parameter };
            }
            return Function.TryRun(paramReady);
        }
        public string ToText()
        {
            return $"{Function.Name}({Parameter.ToText()})";
        }
        public Word? Simplify()
        {
            var simpparam = Parameter.SuperSimplify(out bool simplified);            
            var ret = Call(Parameter.SuperSimplify(out _));
            if(simplified) ret ??= new FunctionCall(Function, simpparam);
            return ret;
        }

        void Word.FindX(VariableLocator locator)
        {
            locator.Path.Push(this);
            Parameter.FindX(locator);
            if (locator.variable == null) locator.Path.Pop();
            
        }

        public FunctionCall(IFunction head, Word parameter)
        {
            Function = head;
            this.Parameter = parameter;
        }     
    }

}

