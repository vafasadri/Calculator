using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCalc.Exceptions
{
    internal class YouShouldntBeHere : Exception
    {
        public override string Message => "You Shouldn't be here! Stack Trace: " + StackTrace;
        public YouShouldntBeHere() {
        
        }
    }
}
