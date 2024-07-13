using System.Diagnostics;
using System.Text;
using SharpCalc.Components;
//using static SharpCalc.Equations.Equation;

namespace SharpCalc.DataModels
{
    internal enum AddResult
    {
        Exists,Conflicts,Added
    }
    internal class SolvedEquation : IMathNode
    {
        public string TypeName => "Solved Equation";
        internal Variable Variable { get; private set; }        
        internal List<Real>? Equivalents = null;
        internal Complex? NumberValue { get; private set; }

        public string ToText()
        {
            StringBuilder builder = new();
            builder.Append(Variable.ToText());
            builder.Append(" = ");
            if (Equivalents != null)
            {
                foreach (var item in Equivalents)
                {
                    builder.Append(item.ToText());
                    builder.Append(" = ");
                }
            }
            if (NumberValue.HasValue)
            {
                builder.Append(NumberValue.Value);

            }
            else builder.Remove(builder.Length - 3, 3);
            return builder.ToString();
        }
        public AddResult AddValue(Real value)
        {
            if (value is Number n)
            {
                if (NumberValue.HasValue)
                {
                    return NumberValue.Value == n.Value ? AddResult.Exists : AddResult.Conflicts;
                }
                else
                {
                    NumberValue = n.Value;
                    return AddResult.Added;
                }
            }
            else
            {
                Equivalents ??= new();
                Equivalents.Add(value);
                return AddResult.Added;
            }
        }
        public bool Contains(Real word)
        {
            if (word is Number n) return NumberValue.HasValue && n.Value == NumberValue.Value;
            return Equivalents != null && Equivalents.Contains(word, Equator.Instance);
        }
        public SolvedEquation(Variable variable, Real word)
        {
            Debug.Assert(variable.Equation == null);
            variable.Equation = this;
            Variable = variable;
            if (word is Number n) NumberValue = n.Value;
            else
            {
                Equivalents = new()
                {
                    word
                };
            }
        }
    }
}
