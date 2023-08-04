namespace SharpCalc.Exceptions
{
    internal class UnknownSymbolError : ApplicationException
    {
        string symbol;
        public override string Message => $"Unable to translate symbol: {symbol}";
        public UnknownSymbolError(string symbol)
        {
            this.symbol = symbol;
        }
    }
}
