using SharpCalc.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCalc.Operators.Logic
{
    internal class Condition : SingleOperatorBase, Statement
    {
        public static readonly new SingleOperatorMetadata Metadata = new(typeof(Condition), [])
        {
            Name = "Condition",
            Precedence = 5,
            Creator = (left, right) => new Condition((Statement) left!, (Statement)right!),
            OperandsSwappable = false,            
        };
        public new Statement Left => (Statement) base.Left!;
        public new Statement Right => (Statement) base.Right!;
        public Condition(Statement left, Statement right) : base(left, right)
        {  
            
        }

        public override bool Equals(IMathNode? other) => throw new NotImplementedException();
        public override string Render()
        {
            return WrapMember(Left) + " => " + WrapMember(Right);
        }
        public override IMathNode? SimplifyInternal()
        {
            return new Or([new Not(Left),Right ]);
        }
    }
}
