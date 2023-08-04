namespace SharpCalc.DataModels;
internal partial class RuntimeFunction : IFunction
{
    readonly Parameter[] Parameters;
    readonly Word Body;
    public string Name { get; private set; }
    public int ParameterCount => Parameters.Length;
    string Word.TypeName => "Runtime Function";   
    public Word Run(IReadOnlyList<Word> paramlist)
    {       
        if (paramlist.Count < Parameters.Length)
        {
            throw new Exceptions.InsufficientParametersError(Parameters.Length, paramlist.Count);
        }

        for (int i = 0; i < Parameters.Length; i++)
        {
            Parameters[i].Value = paramlist[i];
        }
        // this would create a new copy of body
        // and parameter values won't be lost in the
        // answer when we change them back to null
        var newBody = Body.SuperSimplify(out _);
        for (int i = 0; i < Parameters.Length; i++)
        {
            Parameters[i].Value = null;
        }
        return newBody;
    }
    // create in runtime
    private RuntimeFunction(string head, LinkedList<object> body, List<string> paramNames,MathParser parent)
    {
        Parameters = new Parameter[paramNames.Count];
        for (int i = 0; i < Parameters.Length; i++)
        {
            Parameters[i] = new(paramNames[i]);
        }
        this.Body = Translator.Classify(body, parent.dataManager, Parameters).SuperSimplify(out _);
        this.Name = head;
    }
    // create in Compile time
    internal RuntimeFunction(string name,Word body, Parameter[] parameters) {
        Name = name;
        Body = body;
        Parameters = parameters;
    }
    public string Print()
    {
        // f(a,b,c,...) => function body
        return $"{Name}({string.Join(", ", from n in Parameters select n.Name)}) => {Body.ToText()}";
    }
    readonly static LinkedList<object> CreationSyntax = Serializer.ToObjectSeries("{&}({*}) = {*}", true);
    public static RuntimeFunction? CreateFunction(LinkedList<object> x, MathParser sender)
    {
       
        List<object> extraction = new();
        if (!Serializer.MatchSyntax(x, CreationSyntax, extraction)) return null;
        var head = (string)extraction[0];
        if (sender.dataManager.GetData(new ShallowName(head)) is IFunction) return null;
        var paramList = (LinkedList<object>)extraction[1];
        var translation = (LinkedList<object>)extraction[2];
        // this is a function, proven! start initalizing
        try
        {
            var parameters = Utilities.SplitByComma(paramList);
            List<string> paramNames = new();
            foreach (var p in parameters)
            {
                if (p.Count == 1 && p.First?.Value is string pname)
                {
                    paramNames.Add(pname);
                }
                else throw new Exceptions.ExpectingError("a Name", "in new function parameter list", null);
            }
            RuntimeFunction makefunc = new(head, translation, paramNames, sender);
            sender.dataManager.AddFunction(makefunc);
            return makefunc;
        }
        catch
        {
            return null;
        }
    }

    public Word? TryRun(IReadOnlyList<Word> paramlist)
    {
        return Run(paramlist);
    }
    // functions might have variables in their body but their body is raw    
    void Word.FindX(VariableLocator locator)
    {       
    }
}
