namespace SharpCalc.Operators
{
    internal interface IOperator : Word
    {
        OperatorMetadata Metadata { get; }
        string Word.TypeName => Metadata.Name;       
    }
    interface IArithmeticOperator : IOperator, IContent
    {

    }
    internal interface IOperatorGroup : IOperator
    {
        new OperatorGroupMetadata Metadata { get; }
        OperatorMetadata IOperator.Metadata => Metadata;
        Word Convert(Word word,Symbol symbol);
        void AddOperand(Word operand);
        void Seal();
        bool IsSealed { get; }   
    }
}
