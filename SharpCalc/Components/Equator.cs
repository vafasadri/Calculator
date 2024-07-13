using SharpCalc.DataModels;
using SharpCalc.Operators;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace SharpCalc.Components;

public class Equator : IEqualityComparer<Real>
{
    public readonly static IEqualityComparer<Real> Instance = new Equator();
    static public bool Equals(Real left, Real right)
    {
        if (left is Number nl && right is Number nr) return nl.Value == nr.Value;
        if (ReferenceEquals(left, right)) return true;
        if (left is Variable { Equation: not null } lv && lv.Equation.Contains(right)) return true;
        if (right is Variable { Equation: not null } rv && rv.Equation.Contains(left)) return true;

        if (left is FunctionCall lf && right is FunctionCall rf) return lf.Left == rf.Left && Equator.Equals(lf.Right, rf.Right); 
        return false;
    }

    bool IEqualityComparer<Real>.Equals(Real? x, Real? y)
    {
        if (x == null || y == null) return ReferenceEquals(x, y);
        else return Equals(x, y);
    }

    public int GetHashCode([DisallowNull] Real obj)
    {
        throw new NotImplementedException();
    }
    private Equator()
    {
    }
}
