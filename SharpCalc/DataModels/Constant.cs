using SharpCalc.Components;
using SharpCalc.Operators;

namespace SharpCalc.DataModels
{
    internal class Constant : Variable
    {
        public override string TypeName => "Constant";            
        public Constant(string name, Complex value) : base(name)
        {         
            Value = new Number(value);
        }
    }
}
