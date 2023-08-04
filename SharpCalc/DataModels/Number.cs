namespace SharpCalc.DataModels
{
    /// <summary>
    /// Wrapper class for <see cref="Double"/> which turns it into a <see cref="Word"/> 
    /// </summary>
    internal struct Number : IContent
    {
        public double Value { get; }
        string Word.TypeName => "Number";
        public string ToText()
        {
            return Value.ToString();
        }
        Word? Word.Simplify()
        {
            return null;
        }     
        void Word.FindX(VariableLocator locator) { }

        Word IContent.Derivative(Proxy x)
        {
            return new Number(0);
        }

        public Number(double value)
        {
            Value = value;
        }
    }
}
