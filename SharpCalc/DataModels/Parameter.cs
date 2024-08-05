namespace SharpCalc.DataModels;
partial class RuntimeFunction
{
    internal class Parameter : Variable
    {
        public override string TypeName => "Function Parameter";
        public Parameter(string name) : base(name)
        {
        }
    }
}
