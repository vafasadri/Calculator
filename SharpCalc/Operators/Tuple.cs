using SharpCalc.Components;
using SharpCalc.DataModels;

namespace SharpCalc.Operators
{
    internal class Tuple : OperatorGroupBase
    {

        public static readonly new OperatorGroupMetadata Metadata = new(typeof(Tuple), [])
        {
            Name = "Parameter List",
            Precedence = 5,
            EmptyCreator = () => new Tuple(),
            FullCreator = (factors) => new Tuple(factors),
            EmptyValue = null,
            UnpackSelfType = false,
            OperandsSwappable = false
        };
        public static readonly Tuple Empty = new([]);
        public override string Render()
        {
            return string.Join(", ", Factors.Cast<Scalar>().Select(WrapMember));
        }
        public override IMathNode? SimplifyInternal()
        {
            IMathNode[] simplifiedFactors = pool.Rent(Factors.Count);
            var simplified = Utilities.SimplifyAny(Factors, simplifiedFactors);
            if (simplified) return new Tuple(simplifiedFactors.Take(Factors.Count));
            else return null;
        }

        public override bool Equals(IMathNode? other)
        {
            if (Factors.Count == 1 && Equator.Equals(Factors[0], other)) return true;
            else if (other is Tuple tuple)
            {
                return Factors.SequenceEqual(tuple.Factors, Equator.Instance);
            }
            return false;
        }

        public Tuple(IEnumerable<IMathNode> nodes) : base(nodes) { }
        public Tuple() : base() { }
    }
}
