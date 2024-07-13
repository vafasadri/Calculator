using SharpCalc.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCalc.Exceptions
{
    internal class SigmaInvalidFunctionException : ApplicationException
    {
        IFunction func;
        public override string Message => $"the function {func.Name} takes {func.ParameterCount} parameter(s) and cannot be used in sigma summations";
        public SigmaInvalidFunctionException(IFunction func)
        {
            this.func = func;
        }
    }
}
