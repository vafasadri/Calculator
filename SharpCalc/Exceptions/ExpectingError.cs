namespace SharpCalc.Exceptions
{
    internal class ExpectingError : ApplicationException
    {
        string what;
        string where;
        Word? provided;
        public override string Message => $"Expected {what} {where}, '{(provided == null ? "Nothing" : provided.TypeName)}' provided";
        public ExpectingError(string What, string Where, Word? Provided)
        {
            what = What;
            where = Where;
            provided = Provided;
        }
    }
}
