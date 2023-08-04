using SharpCalc.DataModels;
using System.Diagnostics.CodeAnalysis;

namespace SharpCalc;

public class Equator : IEqualityComparer<Word>
{
    public readonly static IEqualityComparer<Word> Instance = new Equator();
    static public bool Equals(Word left, Word right)
    {
        if (left is Number nl && right is Number nr) return nl.Value == nr.Value;     
        if (ReferenceEquals(left, right)) return true;
        if (left is Variable {Equation : not null } lv && lv.Equation.Contains(right)) return true;
        if (right is Variable {Equation: not null } rv && rv.Equation.Contains(left)) return true;
        return false;
    }

    bool IEqualityComparer<Word>.Equals(Word? x,Word? y)
    {
        if (x == null || y == null) return ReferenceEquals(x, y);
        else return Equals(x, y);
    }

    public int GetHashCode([DisallowNull] Word obj)
    {
        throw new NotImplementedException();
    }
    private Equator()
    {
    }
}
