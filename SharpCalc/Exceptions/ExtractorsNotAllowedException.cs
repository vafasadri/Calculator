using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCalc.Exceptions
{
    internal class ExtractorsNotAllowedException : ApplicationException
    {
        public override string Message => "Extractors are not allowed";
    }
}
