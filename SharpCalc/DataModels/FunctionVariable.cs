using SharpCalc.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCalc.DataModels
{
    internal class FunctionVariable : Proxy, IFunction
    {
        public new IFunction? Value { get => (IFunction?) base.Value; set => base.Value = value; }
        public FunctionVariable(string name) : base(name)
        {

        }

        public int ParameterCount => throw new NotImplementedException();

        public Scalar Differentiate(ReadOnlySpan<IMathNode> parameter) => throw new NotImplementedException();
        public IFunction GetDerivative() => throw new NotImplementedException();
        public Scalar? Reverse(Scalar factor, Scalar target, ReadOnlySpan<IMathNode> paramlist) => throw new NotImplementedException();
        public Scalar? Run(ReadOnlySpan<IMathNode> paramlist) => throw new NotImplementedException();
        public Complex RunFast(ReadOnlySpan<Complex> paramlist) => throw new NotImplementedException();
    }
}
