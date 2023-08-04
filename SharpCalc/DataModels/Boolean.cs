namespace SharpCalc.DataModels
{
    internal record struct Boolean : Word
    {
        internal bool Value { get; }
        string Word.TypeName => "Boolean";

        string Word.ToText()
        {
            return Value.ToString();
        }
        Word? Word.Simplify()
        {
            return null;
        }
        void Word.FindX(VariableLocator locator) => throw new Exceptions.EquationChildError();      
        public Boolean(bool value)
        {
            Value = value;
        }
    }
}
