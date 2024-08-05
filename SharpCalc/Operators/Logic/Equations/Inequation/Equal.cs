/*
namespace SharpCalc.Operators.Comparative
{
    internal class Equal : ComparativeBase
    {
        public static readonly VisibleOpBase Handle = new Equal();
        public static readonly OperatorInfo InfoBuffer = new(4)
        {
            UsesRight = true,
            UsesLeft = true,
            Name = "Equals"
        };
        public override OperatorInfo Info => InfoBuffer;
        protected override SymbolCode Symbol => SymbolCode.Equal;
        protected override VisibleOpBase Opposite => NonEqual.Handle;

        protected override bool Act(double a, double b)
        {
            return a == b;
        }
    }
}*/
