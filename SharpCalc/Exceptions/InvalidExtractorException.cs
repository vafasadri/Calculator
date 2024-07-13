using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCalc.Exceptions
{
    internal class InvalidExtractorException : ApplicationException
    {
        public override string Message => "Invalid Extractor";
    }
}
