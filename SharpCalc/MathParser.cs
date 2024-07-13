using SharpCalc.Components;
using SharpCalc.DataModels;
using SharpCalc.Equations;
using SharpCalc.Operators;

namespace SharpCalc;
/// <summary>
/// main class of the library, converts strings containing math expressions into <see cref="IMathNode"/> objects, simplifies them and returns the result 
/// </summary>
public class MathParser
{
    internal readonly DataBank dataManager;
    public MathParser()
    {
        dataManager = new();
    }


    // string -> lexical series -> operator objects -> simplification
    public IMathNode Run(string value, out bool printInDetails)
    {
        var byteCode = LexicalAnalyzer.ToLexicalSeries(value, false);
        IMathNode? result = RuntimeFunction.CreateFunction(byteCode, this);

        result ??= Classifier.Classify(byteCode, dataManager, null);
        printInDetails = result is not OperatorBase || result is Equation;
        if (result is Real real) result = real.SuperSimplify(out _);

        return result;


    }
}
