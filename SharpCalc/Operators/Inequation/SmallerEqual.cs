//namespace SharpCalc.Operators.Comparative
//{
//    internal class SmallerEqual : ComparativeBase
//    {
//        public static readonly VisibleOpBase Handle = new Equal();
//        public static readonly OperatorInfo InfoBuffer = new(4)
//        {
//            UsesRight = true,
//            UsesLeft = true,
//            Name = "Smaller Equal"
//        };
//        public override OperatorInfo Info => InfoBuffer;
//        protected override SymbolCode Symbol => SymbolCode.SmallerOrEqual;
//        protected override VisibleOpBase Opposite => Greater.Handle;
//        protected override bool Act(double a, double b)
//        {
//            return a <= b;
//        }
//    }
//}
