namespace SharpCalc.Exceptions
{
    internal class InsufficientParametersError : ApplicationException
    {
        int needed;
        int provided;
        public override string Message => $"Expected {(needed == 0 ? "no" : needed)} {(needed == 1 ? "parameter" : "parameters")}, {provided} provided!";
        public InsufficientParametersError(int needed, int provided)
        {
            this.needed = needed;
            this.provided = provided;
        }
    }
}
