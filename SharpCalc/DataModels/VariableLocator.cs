using SharpCalc.Operators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCalc.DataModels
{
    internal class VariableLocator
    {
        internal Variable? variable = null;
        internal readonly Stack<IOperator> Path = new();
        public VariableLocator()
        {
        }
    }
}
