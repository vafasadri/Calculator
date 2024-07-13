using SharpCalc.DataModels;

namespace SharpCalc.Exceptions
{
    internal class ExpectingError : ApplicationException
    {
        readonly string What;
        readonly string Where;
        readonly IMathNode? Provided;
        public override string Message => $"Expected {What} {Where}, '{(Provided == null ? "Nothing" : Provided.TypeName)}' provided";
        public ExpectingError(string what, string where, IMathNode? provided)
        {
            this.What = what;
            this.Where = where;
            this.Provided = provided;
        }
    }
}
