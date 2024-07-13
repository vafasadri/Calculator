using SharpCalc.Components;
using SharpCalc.DataModels;
using SharpCalc.Operators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SharpCalc.Equations
{
    internal class EquationResult : Real
    {
        string proxyName;
        IMathNode Value;       
        string IMathNode.TypeName => "Equation Result";     
        public string Print()
        {
            return $"{proxyName} = {Value.ToText()}";
        }
        Real Real.Differentiate()
        {
            throw new NotImplementedException();
        }

        void Real.EnumerateVariables(ISet<Proxy> variables)
        {
            throw new NotImplementedException();
        }

        Real? Real.Simplify()
        {

            var simpright = Value.TrySimplify(out bool did);
            if (did) return new EquationResult(proxyName,simpright);
            else return null;
        }

        string IMathNode.ToText()
        {
            throw new NotImplementedException();
        }

        bool Real.ContainsVariable(Proxy variable) => throw new NotImplementedException();

        public EquationResult(string  proxyName, IMathNode value)
        {
            this.proxyName = proxyName;
            this.Value = value;
        }
    }
}
