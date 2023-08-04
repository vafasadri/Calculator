using SharpCalc.DataModels;
using System.Diagnostics;

namespace SharpCalc.Operators
{
    internal class Tuple : IOperatorGroup
    {
        public List<Word> Content { get; }

        public static readonly OperatorGroupMetadata Metadata = OperatorGroupMetadata.Register
        (
            "Parameter List",5,
            () => new Tuple()
        );
        OperatorGroupMetadata IOperatorGroup.Metadata => Metadata;
        public bool IsSealed { get; private set; }

        public string ToText()
        {
            return string.Join(", ", Content.Select(n => n.ToText()));
        }
        public Word? Simplify()
        {
            if (Utilities.SimplifyAny(Content, out var result))
            {
                return new Tuple(result);
            }
            else return null;
        }

        void Word.FindX(VariableLocator locator)
        {
            Utilities.IterateFactors(locator, Content);
        }

        void IOperatorGroup.AddOperand(Word operand)
        {
            if (IsSealed) throw new Exception("operator is sealed");
            Content.Add(operand);
        }
        public void Seal()
        {
            IsSealed = true;          
        }

        public Word Convert(Word word, Symbol symbol)
        {
            Debug.Assert(symbol == Symbol.Comma || symbol == Symbol.Null);
            return word;          
        }
        public Tuple(IEnumerable<Word> words)
        {
            Content = words.ToList();
        }
        public Tuple()
        {
            Content = new();
        }
        public Tuple(params Word[] words) : this(words as IEnumerable<Word>)
        {

        }
       
    }
}
