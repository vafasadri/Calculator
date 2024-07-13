using SharpCalc.Components;
using SharpCalc.Operators;

namespace SharpCalc.DataModels
{
    internal class Constant : Proxy
    {
        public override string TypeName => "Constant";
        public override bool HasValue => true;      
        public Constant(string name, Complex value) : base(name)
        {         
            Value = new Number(value);
        }
    }
}
