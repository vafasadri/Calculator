using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCalc.Exceptions
{
    [Obsolete]    
    internal class CustomError : ApplicationException
    {       
        public override string Message { get; }
        public CustomError(string message)
        {
            Message = message;
        }
    }
}
