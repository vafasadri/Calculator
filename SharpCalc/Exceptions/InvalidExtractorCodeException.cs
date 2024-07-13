using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCalc.Exceptions
{
    internal class InvalidExtractorCodeException : ApplicationException
    {
        readonly char ExtractorCode;
        public override string Message => $"Invalid extractor code: '{ExtractorCode}'";
        public InvalidExtractorCodeException(char extractorCode) { 
        ExtractorCode = extractorCode;
        }
    }
}
