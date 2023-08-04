using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCalc.Exceptions
{
    internal class SealedOperatorException : ApplicationException
    {
        public override string Message => "This Operator has been sealed";
    }
}
