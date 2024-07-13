using SharpCalc.Components;
using SharpCalc.DataModels;

namespace SharpCalc.Operators
{
    internal class FunctionCall : SingleOperatorBase
    {
        public static readonly SingleOperatorMetadata MetadataValue = new(
           "Function Call",
           -1,
            (left, right) => new FunctionCall((IFunction)left,right),
            Array.Empty<ISimplification>()
        );
        private IFunction Function => (IFunction)Left!;
        private IMathNode Parameter => Right!;
        public override SingleOperatorMetadata Metadata => MetadataValue;
        static IReadOnlyList<IMathNode> UnpackTuple(IMathNode word)
        {
            if (word is Tuple tuple)
            {
                return tuple.Factors;
            }
            else return new IMathNode[] { word };
        }
        public override Real? Simplify()
        {
            var x = Parameter.TrySimplify(out _);
            return Function.TryRun( UnpackTuple(x)) ?? base.Simplify();
        }
        public override string ToText()
        {
            return $"{Function.Name}({Parameter.ToText()})";
        }

        public override Real Differentiate()
        {
            if (Function.ParameterCount > 1) throw new NotImplementedException();
            return Function.Differentiate((Real) UnpackTuple(Parameter)[0]);
        }

        public override Real Reverse(Real factor, Real target)
        {
            return Function.Reverse(factor, target, UnpackTuple(Parameter));           
        }

        public FunctionCall(IFunction head, IMathNode parameter) : base(head, parameter) { }
    }

    
}

