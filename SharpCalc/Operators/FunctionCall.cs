using SharpCalc.Components;
using SharpCalc.DataModels;
using System.Buffers;

namespace SharpCalc.Operators
{
    internal class FunctionCall : SingleOperatorBase, IScalarOperator
    {
        public static readonly new SingleOperatorMetadata Metadata = new(typeof(FunctionCall), [])
        {
            Name = "Function Call",
            Precedence = -1,
            Creator = (left, right) => new FunctionCall((IFunction)left!, right!),
            OperandsSwappable = false,
        };
        private IFunction Function => (IFunction)Left!;
        private readonly IMathNode[] Parameters;

        public override IMathNode? SimplifyInternal()
        {
            var buffer = ArrayPool<IMathNode>.Shared.Rent(Parameters.Length);
            var did = Utilities.SimplifyAny(Parameters, buffer);
            var output = Function.Run(new Span<IMathNode>(buffer, 0, Parameters.Length));
            if (did) output ??= new FunctionCall(Function, buffer.Take(Parameters.Length));
            return output;
        }

        public override string Render()
        {
            return $"{Function.Name}({string.Join(" , ", from param in Parameters select param.Render())})";
        }

        public Scalar Differentiate()
        {
            //if (Function.ParameterCount > 1) throw new NotImplementedException();
            return Function.Differentiate(Parameters);
        }

        public Scalar? Reverse(Scalar factor, Scalar target)
        {
            return Function.Reverse(factor, target, Parameters);
        }

        public Complex ComputeNumerically()
        {
            Span<Complex> vals = stackalloc Complex[Parameters.Length];
           
            for (int i = 0; i < Parameters.Length; i++)
            {
                vals[i] = ((Scalar)Parameters[i]).ComputeNumerically();
            }
            return Function.RunFast(vals);
        }

        public Scalar? GetParentFactor(Variable variable)
        {
            if (this.ContainsVariable(variable))
                return (Scalar) Parameters.Where(n => ((Scalar)n).ContainsVariable(variable)).First();
            else return null;
        }

        public void EnumerateVariables(ISet<Variable> variables)
        {
            foreach (var param in Parameters)
            {
                (param as Scalar)?.EnumerateVariables(variables);
            }
        }
        public bool ContainsVariable(Variable variable)
        {
            foreach (var param in Parameters)
            {
                if (param is Scalar ss && ss.ContainsVariable(variable)) return true;
            }
            return false;
        }

        public override bool Equals(IMathNode? other) => false;// other is FunctionCall call && Equator.Equals(call.Parameter, Parameter) && Equator.Equals(call.Function, Function);            
        public FunctionCall(IFunction head, IMathNode parameter) : base(head, null)
        {
            static IMathNode[] UnpackTuple(IMathNode param)
            {
                if (param is Tuple tuple)
                {
                    return tuple.Factors.ToArray();
                }
                else return [param];
            }
            Parameters = UnpackTuple(parameter);
            if (head.ParameterCount != -1 && Parameters.Length != head.ParameterCount) throw new Exceptions.CustomError("Incorrect number of parameters");
        }
        public FunctionCall(IFunction head, IEnumerable<IMathNode> nodes) : base(head, null)
        {
            Parameters = nodes.ToArray();
            if (head.ParameterCount != -1 && Parameters.Length != head.ParameterCount) throw new Exceptions.CustomError("Incorrect number of parameters");
        }
        public FunctionCall(IFunction head, ReadOnlySpan<IMathNode> nodes) : base(head, null)
        {
            Parameters = nodes.ToArray();
            if (head.ParameterCount != -1 && Parameters.Length != head.ParameterCount) throw new Exceptions.CustomError("Incorrect number of parameters");
        }
    }


}

