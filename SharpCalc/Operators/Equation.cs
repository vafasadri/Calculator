using SharpCalc.DataModels;

namespace SharpCalc.Operators;
internal class Equation : IEquation
{
    internal readonly Word left;
    internal readonly Word right;
    internal double? NumberValue { get; private set; }

    public static readonly SingleOperatorMetadata Metadata = SingleOperatorMetadata.Register
    (
      "Equation", 4,
        (left, right) => new Equation(left, right)

    );
    OperatorMetadata IOperator.Metadata => Metadata;
    Word? Word.Simplify()
    {
        var l = left.SuperSimplify(out _);
        var r = right.SuperSimplify(out _);
        if (l is Number ln && r is Number nr) return new DataModels.Boolean(ln.Value == nr.Value);
        if (Equator.Equals(l, r)) return new DataModels.Boolean(true);
        else
        {
            var solved = SolveForX();
            if (solved != null) return solved;
        }
        return null;
    }
    void Word.FindX(VariableLocator locator) => throw new Exceptions.EquationChildError();
    public string ToText()
    {
        return $"{left.ToText()} = {right.ToText()}";
    }
    public Equation(Word left, Word right)
    {
        this.left = left;
        this.right = right;
    }
    internal enum AddResult
    {
        Added, Exists, Conflicts
    };

    public SolvedEquation? SolveForX()
    {

        var locator = new VariableLocator();
        left.FindX(locator);
        var path = locator.Path.Reverse().ToArray();
        if (locator.variable != null)
        {
            if (locator.Path.Count > 0)
            {
                throw new NotImplementedException();

            }
            else
            {
                if (locator.variable.Equation != null)
                {
                    return locator.variable.Equation.AddValue(right) != AddResult.Conflicts ? locator.variable.Equation : null;
                }
                return new SolvedEquation(locator.variable, right);
            }
        }
        return null;
    }  
}
