//namespace SharpCalc.Operators.Comparative
//{
//    internal class Smaller : ComparativeBase
//    {
//        public static readonly VisibleOpBase Handle = new Equal();
//        public static readonly OperatorInfo InfoBuffer = new(4)
//        {
//            UsesRight = true,
//            UsesLeft = true,
//            Name = "Smaller"
//        };
//        public override OperatorInfo Info => InfoBuffer;
//        protected override SymbolCode Symbol => SymbolCode.Smaller;
//        protected override VisibleOpBase Opposite => GreaterEqual.Handle;
//        protected override bool Act(double a, double b)
//        {
//            return a < b;
//        }
//    }
//}
