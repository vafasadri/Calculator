using SharpCalc.Components;
using SharpCalc.DataModels;
using SharpCalc.Equations;
using SharpCalc.Operators;
using System.Linq.Expressions;
using System.Reflection;

namespace SharpCalc;
/// <summary>
/// main class of the library, converts strings containing math expressions into <see cref="IMathNode"/> objects, simplifies them and returns the result 
/// </summary>
public class MathParser
{
    internal readonly DataBank dataManager;
    internal readonly Concluder concluder;
    private readonly static LinkedList<object> CreationSyntax = LexicalAnalyzer.ToLexicalSeries("{*} : {&}", true);
    public MathParser()
    {
        dataManager = new();
        concluder = new(dataManager);
    }
    public IEnvironmentVariable RegisterEnvironment(string name)
    {
        var variable = new EnivronmentVariable(name);

        if (dataManager.AddData(variable))
        {
            return variable;
        }
        else throw new Exception();
    }
    // string -> lexical series -> operator objects -> simplification
    public IMathNode Run(string value, out bool printInDetails)
    {       
        var byteCode = LexicalAnalyzer.ToLexicalSeries(value, false);
        
        IMathNode? result = DefineSpecialVariable(byteCode);

        result ??= RuntimeFunction.CreateFunction(byteCode, this);

        result ??= Classifier.Classify(byteCode, dataManager, null);
        
        printInDetails = result is not OperatorBase || result is Equal;
        return result.Simplify(out _);
    }
    static readonly string[] typenames = { "scalar","bool" };
    Proxy DefineSpecialVariable(LinkedList<object> x)
    {
        List<object> extraction = new();
        if (!LexicalAnalyzer.MatchSyntax(x, CreationSyntax, extraction)) return null;         
        var nameList = (LinkedList<object>)extraction[0];
        var typename = (string)extraction[1];
        
        List<string> varNames = new();
        foreach (var p in Utilities.SplitByComma(nameList))
        {
            if (p.Count == 1 && p.First?.Value is string pname)
            {
                varNames.Add(pname);
            }
            else throw new Exceptions.ExpectingError("a Name", "variable definition name list", null);
        }
        if (varNames.Count < 1) throw new Exceptions.CustomError("At least one variable required");
        Proxy? output = null;
        int type = Array.IndexOf(typenames, typename);
        if (type == -1) throw new Exceptions.CustomError("Invalid variable type");
        foreach (var item in varNames)
        {
            Proxy? v = null;
            switch (type)
            {
                case 0:
                    v = new Variable(item);
                    break;
                case 1:
                    v = new BooleanVariable(item);
                    break;                 
            }
            output ??= v;
            if(!dataManager.AddData(v!)) throw new Exceptions.CustomError("Redefinition");
        }
        return output!;
    }
}
