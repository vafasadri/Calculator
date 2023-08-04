using SharpCalc.DataModels;
using SharpCalc.Operators.Arithmetic;

namespace SharpCalc;
/// <summary>
/// main class of the library, converts strings containing math expressions into <see cref="Word"/> objects, simplifies them and returns the result 
/// </summary>
public class MathParser
{
    internal readonly DataBank dataManager;
    public MathParser()
    {
        dataManager = new();
    }
    // string -> object series -> operator objects -> simplification
    public Word Run(string value, out bool isEquated)
    {        
        var byteCode = Serializer.ToObjectSeries(value, false);
        Word? result = RuntimeFunction.CreateFunction(byteCode, this);
        result ??= Translator.Classify(byteCode, dataManager, null);
        return result.SuperSimplify(out isEquated);
    }
}
