//namespace SharpCalc.Operators.Comparative
//{
//    internal class NonEqual : ComparativeBase
//    {
//        public static readonly VisibleOpBase Handle = new Equal();
//        public static readonly OperatorInfo InfoBuffer = new(4)
//        {
//            UsesRight = true,
//            UsesLeft = true,
//            Name = "Not Equals"
//        };
//        public override OperatorInfo Info => InfoBuffer;
//        protected override SymbolCode Symbol => SymbolCode.NonEqual;
//        protected override VisibleOpBase Opposite => Equal.Handle;
//        protected override bool Act(double a, double b)
//        {
//            return a != b;
//        }
//    }
//}
