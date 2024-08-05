using SharpCalc.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCalc.Operators.Logic
{
    internal abstract class LogicOperatorGroup : OperatorGroupBase
    {
        public override void AddOperand(IMathNode operand)
        {
            if (operand is not Statement) throw new Exceptions.ExpectingError("a statement", $"in operator '{Metadata.Name}'", operand);
            base.AddOperand(operand);
        }
        protected LogicOperatorGroup() : base() { }
        protected LogicOperatorGroup(IEnumerable<Statement> operands) : base(operands) { }
    }
}
