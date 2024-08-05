namespace SharpCalc.DataModels
{
    internal readonly struct Bool : IDataModel,Statement
    {
        public static readonly Bool True = new Bool("true",true);
        public static readonly Bool False = new Bool("false",false);
        internal bool Value { get; }
        string IMathNode.TypeName => "Boolean";
        public string Name { get; }
        IMathNode? IMathNode.SimplifyInternal() => null;
        public bool Equals(IMathNode? other) => other is Bool b && b.Value == Value;

        private Bool(string name, bool value)
        {
            Name = name;
            Value = value;
        }
    }
}
