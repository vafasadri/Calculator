using SharpCalc.Operators;

namespace SharpCalc.DataModels
{
    internal class Constant : Proxy, IContent, IDataModel, IValued
    {
        public override string TypeName => "Constant";
        public override bool HasValue => true;
        // public string Name { get; }

        //public Word Value { get; }

        //string Word.ToText()
        //{
        //    return Name;
        //}
        //  Word? Word.Simplify() => Value;
        // void Word.FindX(VariableLocator locator) { }

        //Word IContent.Derivative(Variable x)
        //{
        //    return Value;
        //}

        public Constant(string name, double value) : base(name)
        {         
            Value = new Number(value);
        }
    }
}
