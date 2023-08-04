namespace SharpCalc.DataModels
{
    internal class Variable : Proxy
    {
        public override bool HasValue => Equation != null;
        internal SolvedEquation? Equation = null;
        public override string TypeName => "Variable";
        public override Word? Value => Equation?.NumberValue?.ToWord() ?? Equation?.Equivalents?.FirstOrDefault(n => !ReferenceEquals(n, this));
        public override void FindX(VariableLocator locator)
        {
            locator.variable = this;
        }
         
        public Variable(string name) : base(name)
        {
        }
        
    }
}
