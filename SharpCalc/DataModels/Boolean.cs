namespace SharpCalc.DataModels
{
    internal readonly record struct Boolean : Real
    {
        internal bool Value { get; }
        string IMathNode.TypeName => "Boolean";
        string IMathNode.ToText()
        {
            return Value.ToString();
        }
        Real Real.Differentiate() => null;
        void Real.EnumerateVariables(ISet<Proxy> variables) { }
        Real? Real.Simplify() => null;
        bool Real.ContainsVariable(Proxy variable) => throw new NotImplementedException();

        public Boolean(bool value)
        {
            Value = value;
        }
    }
}
