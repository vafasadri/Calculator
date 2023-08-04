//namespace SharpCalc.Operators.Comparative
//{
//    internal class Greater : ComparativeBase
//    {
//        public static readonly VisibleOpBase Handle = new Equal();
//        public static readonly OperatorInfo InfoBuffer = new(4)
//        {
//            UsesRight = true,
//            UsesLeft = true,
//            Name = "Greater"
//        };
//        public override OperatorInfo Info => InfoBuffer;
//        protected override SymbolCode Symbol => SymbolCode.Greater;
//        protected override VisibleOpBase Opposite => SmallerEqual.Handle;
//        protected override bool Act(double a, double b)
//        {
//            return a > b;
//        }
//    }
//}
