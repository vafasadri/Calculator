using SharpCalc.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCalc.Operators.Base
{
    internal abstract class ArithmeticSingleOperator : SingleOperatorBase
    {
        protected ArithmeticSingleOperator(IMathNode? left, IMathNode? right) : base(left, right)
        {
        }

        //public override SingleOperatorMetadata Metadata => throw new NotImplementedException();
        //public override Real Derivative(Proxy x) => throw new NotImplementedException();
        //public override Real Reverse(Real factor, Real target) => throw new NotImplementedException();
        //public override string ToText() => throw new NotImplementedException();
        public abstract override Real? Simplify();
    }
    internal abstract class ArithmeticGroupOperator : OperatorGroupBase
    {
        //public override OperatorGroupMetadata Metadata => throw new NotImplementedException();
        public abstract override Real Convert(IMathNode word, Symbol symbol);
        public abstract override Real? Simplify();
        //public override Real Derivative(Proxy x) => throw new NotImplementedException();
        //public override Real Reverse(Real factor, Real target) => throw new NotImplementedException();
        //public override string ToText() => throw new NotImplementedException();
    }
}
