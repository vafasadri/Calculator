using System.Runtime.CompilerServices;
using SharpCalc.Components;

namespace SharpCalc.DataModels
{
    internal class Variable : Proxy
    {
        public override bool HasValue => Equation != null;
        internal SolvedEquation? Equation = null;
        public override string TypeName => "Variable";       
        //public override Real? Value => Equation?.NumberValue?.ToWord() ?? Equation?.Equivalents?.FirstOrDefault(n => !ReferenceEquals(n, this));              
        public Variable(string name) : base(name)
        {
        }
        
    }
}
