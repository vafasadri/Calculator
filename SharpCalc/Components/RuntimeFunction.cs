using SharpCalc.Components;
using SharpCalc.Operators.Arithmetic;
using System.Buffers;
using System.Diagnostics;

namespace SharpCalc.DataModels;
internal partial class RuntimeFunction : IFunction
{
    private readonly static LinkedList<object> CreationSyntax = LexicalAnalyzer.ToLexicalSeries("{&}({*}) = {*}", true);
    readonly Parameter[] Parameters;
    readonly Scalar Body;
    private IFunction? derivativeCache;

    private List<(IReadOnlyList<IMathNode>, Scalar)> RecursionBaseCases = new();
    public string Name { get; private set; }
    public int ParameterCount => Parameters.Length;
    string IMathNode.TypeName => "Runtime Function";

    bool SequenceEquals(IReadOnlyList<IMathNode> x, ReadOnlySpan<IMathNode> y)
    {
        Debug.Assert(x.Count == y.Length);
        for (var i = 0; i < x.Count; i++)
        {
            if (!Equator.Equals((Scalar)x[i], (Scalar)y[i])) return false;
        }
        return true;
    }

    public Scalar? Run(ReadOnlySpan<IMathNode> paramlist)
    {
        foreach (var i in RecursionBaseCases)
        {
            if (SequenceEquals(i.Item1, paramlist))
            {
                return i.Item2;
            }
        }
        if (Body == null) return null;
        Scalar?[] backupBuffer = ArrayPool<Scalar>.Shared.Rent(Parameters.Length);

        for (int i = 0; i < Parameters.Length; i++)
        {
            backupBuffer[i] = Parameters[i].Value;
            Parameters[i].Value = (Scalar)paramlist[i];
        }
        // this would create a new copy of body
        // and parameter values won't be lost in the
        // result when we change them back to null
        var copy = (Scalar)Body.Simplify(out _);
        for (int i = 0; i < Parameters.Length; i++)
        {
            Parameters[i].Value = backupBuffer[i];
        }
        ArrayPool<Scalar>.Shared.Return(backupBuffer);
        return copy;
    }
   
    public string Print()
    {
        // f(a,b,c,...) => function body
        return $"{Name}({string.Join(", ", from n in Parameters select n.Name)}) => {Body.Render()}";
    }

    
    public Scalar Differentiate(ReadOnlySpan<IMathNode> parameter) => Run(parameter)!.Differentiate();
    public Scalar? Reverse(Scalar factor, Scalar target, ReadOnlySpan<IMathNode> paramlist) => null;

    public IFunction GetDerivative()
    {
        if (ParameterCount > 1) throw new Exceptions.CustomError("Cannot take the derivative of a function with more than one parameter. Use differentiation instead!");
        else if (ParameterCount == 0) throw new Exceptions.CustomError("Cannot take the derivative of a function with no parameters. Use differentiation instead!");
        if (derivativeCache != null) return derivativeCache;

        var differential = Divide.Create(Body.Differentiate(), Parameters[0].Differential) as Scalar;
        var newX = new Parameter(Parameters[0].Name);
        Parameters[0].Value = newX;
        var bodyCopy = (Scalar)differential.Simplify(out _);
        Parameters[0].Value = null;
        derivativeCache = new RuntimeFunction(Name + "'", bodyCopy, new Parameter[] { newX });
        return derivativeCache;
    }

    public Complex RunFast(ReadOnlySpan<Complex> paramlist)
    {
        if (paramlist.Length < Parameters.Length)
        {
            throw new Exceptions.InsufficientParametersError(Parameters.Length, paramlist.Length);
        }
        for (int i = 0; i < Parameters.Length; i++)
        {
            Parameters[i].Value = new Number(paramlist[i]);
        }
        // this would create a new copy of body
        // and parameter values won't be lost in the
        // answer when we change them back to null
        var newBody = Body.ComputeNumerically();
        foreach (var item in Parameters) item.Value = null;
        return newBody;
    }

    public bool Equals(IMathNode? other)
    {
        return false;
        throw new NotImplementedException();
    }
    public static RuntimeFunction? CreateFunction(LinkedList<object> x, MathParser sender)
    {
        List<object> extraction = new();
        if (!LexicalAnalyzer.MatchSyntax(x, CreationSyntax, extraction)) return null;
        var head = (string)extraction[0];
        if (sender.dataManager.ContainsName(new ShallowName(head))) return null;
        var paramList = (LinkedList<object>)extraction[1];
        var translation = (LinkedList<object>)extraction[2];
        // this is a function, proven! start initalizing

        var paramNames = Utilities.SplitByComma(paramList).Select(p =>
        {
            if (p.Count == 1 && p.First?.Value is string pname)
            {
                return new Parameter(pname);
            }
            else throw new Exceptions.ExpectingError("a Name", "in new function parameter list", null);
        }).ToArray();
        try
        {
            RuntimeFunction makefunc = new(head, paramNames, translation, sender);
            return makefunc;
        }
        catch
        {
            return null;
        }
    }
    // create in runtime
    private RuntimeFunction(string head, Parameter[] parameters, LinkedList<object> body, MathParser parent)
    {
        this.Name = head;
        this.Parameters = parameters;
        parent.dataManager.AddFunction(this);
        var temp = (Scalar)Classifier.Classify(body, parent.dataManager, Parameters);
        this.Body = (Scalar)temp.Simplify(out _);
    }
    // create in Compile time
    internal RuntimeFunction(string name, Scalar body, Parameter[] parameters)
    {
        Name = name;
        Body = body;
        Parameters = parameters;
    }
    // this one is used for creating simple compile time functions
    internal RuntimeFunction(string name, ReadOnlySpan<string> parameterNames, string body, MathParser? parent = null)
    {
        Name = name;
        Parameters = new Parameter[parameterNames.Length];
        for (int i = 0; i < Parameters.Length; i++)
        {
            Parameters[i] = new(parameterNames[i]);
        }
        var token = LexicalAnalyzer.ToLexicalSeries(body, false);
        Body = (Scalar)Classifier.Classify(token, parent?.dataManager, Parameters);
        Body = (Scalar)Body.Simplify(out _);
    }
}
