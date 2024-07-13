using SharpCalc.DataModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCalc.Components
{
    internal class Differentiate : IFunction
    {
        public static readonly IFunction Instance = new Differentiate();
        public int ParameterCount => 1;

        public string Name => "d";

        Real IFunction.Differentiate(Real parameter) => throw new NotImplementedException();
        public Real Run(IReadOnlyList<IMathNode> paramlist)
        {
            if (paramlist[0] is Real expression)
            {
                //if (paramlist[1] is Proxy variable)
                //{
                    return expression.Differentiate();
                //}
               // else throw new Exceptions.ExpectingError("variable", "in function call to d", paramlist[1]);
            }
            else throw new Exceptions.ExpectingError("real", "in function call to d", paramlist[1]);
        }
        public Real? TryRun(IReadOnlyList<IMathNode> paramlist) => Run(paramlist);
        public Real Reverse(Real factor, Real target, IReadOnlyList<IMathNode> paramlist) => throw new NotImplementedException();

        private Differentiate()
        {

        }
    }
}
