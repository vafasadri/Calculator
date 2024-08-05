using SharpCalc.DataModels;
using SharpCalc.Operators;

namespace SharpCalc.Equations
{
    internal class EquationResult : SingleOperatorBase, Statement
    {        
        public static readonly new SingleOperatorMetadata Metadata = new(typeof(EquationResult), [])
        {
            Name = "Equation",
            Precedence = 5,
            Creator = (left, right) => new EquationResult((Variable)left, (Scalar)right),
            OperandsSwappable = false
        };
        public Variable Variable => (Variable)Left!;
        public Scalar Value => (Scalar)Right!;
        public string Print()
        {
            return $"{Variable.Name} = {Value.Render()}";
        }
        public override IMathNode? SimplifyInternal()
        {
            var simpright = (Scalar)Value.Simplify(out bool did);
            if (did) return new EquationResult(Variable, simpright);
            else return null;
        }

        public override string Render()
        {
            return $"{Variable.Name} = {Value.Render()}";
        }

        public override bool Equals(IMathNode? other) => throw new Exceptions.YouShouldntBeHere();

        public EquationResult(Variable proxy, Scalar value) : base(proxy, value)
        {
        }
    }
}
