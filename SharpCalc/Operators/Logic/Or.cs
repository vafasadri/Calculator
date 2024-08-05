using SharpCalc.Components;
using SharpCalc.DataModels;
namespace SharpCalc.Operators.Logic;

internal class Or : LogicOperatorGroup, Statement
{
    public readonly static new OperatorGroupMetadata Metadata = new(typeof(Or), [new OrStatementNot(), new OrStatementStatement(), new OrStatementBoolean()])
    {
        Name = "Logical Or",
        Precedence = 6,
        EmptyCreator = () => new Or(),
        FullCreator = (factors) => new Or(factors.Cast<Statement>()),
        UnpackSelfType = true,
    };
    public override IMathNode Convert(IMathNode word, Symbol symbol) => word;
    public override bool Equals(IMathNode? other) => false;
    public override string Render()
    {
        return string.Join(" | ", Factors.Select(WrapMember));
    }
    public Or() : base() { }
    public Or(IEnumerable<Statement> statements) : base(statements) { }
}
file class OrStatementNot : ISimplification<Statement, Not, Or>
{
    public IMathNode? Simplify(Statement left, Not right)
    {
        if (Equator.Equals(left, right.Operand)) return SharpCalc.DataModels.Bool.True;
        return null;
    }
}
file class OrStatementStatement : ISimplification<Statement, Statement, Or>
{
    public IMathNode? Simplify(Statement left, Statement right)
    {
        if (Equator.Equals(left, right)) return left;
        return null;
    }
}
file class OrStatementBoolean : ISimplification<Statement, SharpCalc.DataModels.Bool, Or>
{
    public IMathNode? Simplify(Statement left, SharpCalc.DataModels.Bool right)
    {
        // return true
        if (right.Value == true) return right;
        // return x
        else return left;
    }
}