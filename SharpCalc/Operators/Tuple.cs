using SharpCalc.DataModels;
using System.Diagnostics;

namespace SharpCalc.Operators
{
    internal class Tuple : OperatorGroupBase
    {

        public static readonly OperatorGroupMetadata MetadataValue = new
        (
            "Parameter List", 5,
            () => new Tuple(),
            (factors) => new Tuple(factors),
            Array.Empty<ISimplification>(),
            new Tuple(Enumerable.Empty<IMathNode>()),
            new Number(double.NaN)
        );
        public override OperatorGroupMetadata Metadata => MetadataValue;
        public override string ToText()
        {
            return string.Join(", ", Factors.Cast<Real>().Select(WrapMember));
        }
        public override IMathNode Convert(IMathNode word, Symbol symbol)
        {
            Debug.Assert(symbol == Symbol.Comma || symbol == Symbol.Null);
            return word;
        }

        public override Real Differentiate()
        {
            throw new NotImplementedException();
        }

        public override Real Reverse(Real factor, Real target)
        {
            throw new InvalidOperationException();
        }

        public Tuple(IEnumerable<IMathNode> words) : base(words) { }
        public Tuple() : base() { }
        public Tuple(params Real[] words) : base(words) { }

    }
}
