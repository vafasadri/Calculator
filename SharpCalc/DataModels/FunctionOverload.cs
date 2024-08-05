using SharpCalc.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCalc.DataModels
{
    class FunctionOverload : IFunction
    {
        readonly List<IFunction?> functions;
        public int ParameterCount => -1;
        public string Name { get; }
        public Scalar Differentiate(ReadOnlySpan<IMathNode> parameter)
        {
            var func = functions[parameter.Length];
            return func!.Differentiate(parameter);
        }

        public bool Equals(IMathNode? other) => false;
        public IFunction GetDerivative()
        {
            var func = functions[1];
            return func!.GetDerivative();
        }
        public Scalar? Reverse(Scalar factor, Scalar target, ReadOnlySpan<IMathNode> paramlist)
        {
            var func = functions[paramlist.Length];
            return func!.Reverse(factor, target, paramlist);
        }
        public Scalar? Run(ReadOnlySpan<IMathNode> paramlist)
        {
            var func = functions[paramlist.Length];
            return func!.Run(paramlist);
        }

        public Complex RunFast(ReadOnlySpan<Complex> paramlist)
        {
            var func = functions[paramlist.Length];
            return func!.RunFast(paramlist);
        }
        public void AddFunction(IFunction function)
        {
            while(functions.Count <= function.ParameterCount)
            {
                functions.Add(null);
            }
            Debug.Assert(functions[function.ParameterCount] == null);
            functions[function.ParameterCount] = function;
        }
        public string Print()
        {
            return string.Join('\n', functions.Where(n => n != null).Select(n => n.Print()));
        }
        public FunctionOverload(string name, IEnumerable<IFunction> funcs)
        {
            this.Name = name;
            var size = funcs.Max(n => n.ParameterCount);
            functions = new List<IFunction?>(Enumerable.Repeat((IFunction) null,size + 1));
            foreach (var item in funcs)
            {
                Debug.Assert(item.Name == name);
                var i = item.ParameterCount;
                Debug.Assert(functions[i] == null);
                functions[i] = item;
            }
        }
    }
    
}
