using SharpCalc.Components;
using SharpCalc.DataModels;
using SharpCalc.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCalc.Functions
{
    internal class Differentiate : IFunction
    {
        public static readonly IFunction Instance = new Differentiate();
        public int ParameterCount => 1;

        public string Name => "d";

        Scalar IFunction.Differentiate(ReadOnlySpan<IMathNode> param) => throw new CustomError("Cannot differentiate the differential function");

        public Scalar Run(ReadOnlySpan<IMathNode> paramlist)
        {
            if (paramlist[0] is Scalar expression)
            {
                return expression.Differentiate();
            }
            else throw new CustomError("Cannot Differentiate a non-scalar");
        }
        public Scalar? TryRun(ReadOnlySpan<IMathNode> paramlist) => Run(paramlist);
        public Scalar Reverse(Scalar factor, Scalar target, ReadOnlySpan<IMathNode> paramlist) => throw new YouShouldntBeHere();
        public IFunction GetDerivative() => throw new CustomError("Cannot differentiate the differential function");
        public Complex RunFast(ReadOnlySpan<Complex> paramlist) => throw new YouShouldntBeHere();

        private Differentiate()
        {

        }
        string IMathNode.Print()
        {
            return "d(x) : the differential function, differentiates the expression inside it";
        }

        public bool Equals(IMathNode? other) => other == this;
    }
}
