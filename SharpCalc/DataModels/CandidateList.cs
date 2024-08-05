using SharpCalc.Components;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCalc.DataModels
{
    internal class CandidateList : Scalar
    {
        public readonly IReadOnlyList<Scalar> Candidates;
        public string TypeName => "List";
        public Complex ComputeNumerically() => throw new NotImplementedException();
        public string Render() => "[" + string.Join(", ", from c in Candidates select c.Render()) +"]";
        bool Scalar.ContainsVariable(Variable variable)
        {
            return Candidates.Any(n => n.ContainsVariable(variable));
        }
        Scalar Scalar.Differentiate()
        {
            return new CandidateList(Candidates.Select(n => n.Differentiate()));
        }
        void Scalar.EnumerateVariables(ISet<Variable> variables)
        {
            foreach (var item in Candidates)
            {
                item.EnumerateVariables(variables);
            }
        }
        IMathNode? IMathNode.SimplifyInternal()
        {
            var simplifiedFactors = ArrayPool<IMathNode>.Shared.Rent(Candidates.Count);
            var simplified = Utilities.SimplifyAny(Candidates, simplifiedFactors);
            IMathNode? output = null;
            if (simplified) output = new CandidateList(simplifiedFactors.Take(Candidates.Count).Cast<Scalar>());
            ArrayPool<IMathNode>.Shared.Return(simplifiedFactors);
            return output;
        }

        public bool Equals(IMathNode? other)
        {
            if (ReferenceEquals(this, other)) return true;
            if(other is not CandidateList candidateList) return false;
            if (candidateList.Candidates.Count != Candidates.Count) return false;
            throw new NotImplementedException();
        }

        public CandidateList(IEnumerable<Scalar> candidates)
        {
            Candidates = candidates.ToArray();
        }
    }
}
