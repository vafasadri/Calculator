using SharpCalc.Components;
using SharpCalc.DataModels;
using SharpCalc.Exceptions;

namespace SharpCalc.Functions
{
    internal class Max : IFunction
    {
        public static readonly IFunction Instance = new Max();
        public int ParameterCount => -1;
        public string Name => "max";
        public Scalar Differentiate(ReadOnlySpan<IMathNode> parameter) => null;
        public bool Equals(IMathNode? other) => throw new NotImplementedException();
        public IFunction GetDerivative() => null;
        public Scalar? Reverse(Scalar factor, Scalar target, ReadOnlySpan<IMathNode> paramlist) => null;
        public Scalar? Run(ReadOnlySpan<IMathNode> paramlist)
        {
            if (paramlist.Length < 2) throw new Exceptions.CustomError("at least 2 arguments are required");
                Complex max;
                try
                {
                    max = ((Scalar)paramlist[0]).ComputeNumerically();
                }
                catch
                {
                    return null;
                }           
                foreach (var e in paramlist)
                {
                    Complex item;
                    try
                    {
                        item = ((Scalar)e).ComputeNumerically();
                    }
                    catch
                    {
                        return null;
                    }                  
                    
                    if (item > max) max = item;
                }
                return new Number(max);           
        }
        public Complex RunFast(ReadOnlySpan<Complex> paramlist)
        {
            if (paramlist.Length < 2) throw new Exceptions.CustomError("at least 2 arguments are required");
            var max = paramlist[0];            
            foreach (var item in paramlist)
            {                
                if (item > max) max = item;
            }
            return max;
        }
        public string Print()
        {
            return "max(a,b,c,...) : Greatest number among a,b,c,...";
        }
        private Max()
        {

        }
    }
}
