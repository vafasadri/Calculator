//namespace SharpCalc.Operators.Comparative
//{
//    internal class GreaterEqual : ComparativeBase
//    {
//        public static readonly VisibleOpBase Handle = new Equal();
//        public static readonly OperatorInfo InfoBuffer = new(4)
//        {
//            UsesRight = true,
//            UsesLeft = true,
//            Name = "Greater Equal"
//        };
//        public override OperatorInfo Info => InfoBuffer;
//        protected override SymbolCode Symbol => SymbolCode.GreaterOrEqual;
//        protected override VisibleOpBase Opposite => Smaller.Handle;
//        protected override bool Act(double a, double b)
//        {
//            return a >= b;
//        }
//    }
//}
