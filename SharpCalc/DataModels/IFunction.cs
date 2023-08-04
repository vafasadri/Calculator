namespace SharpCalc.DataModels;

internal interface IFunction : Word, IDataModel
{

    string Word.TypeName => "Function";
    public int ParameterCount { get; }
    public Word Run(IReadOnlyList<Word> paramlist);
    public Word? TryRun(IReadOnlyList<Word> paramlist);
    string Word.ToText() => Name;
    Word? Word.Simplify() => null;    
}
