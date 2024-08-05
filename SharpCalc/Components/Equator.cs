using SharpCalc.DataModels;
using SharpCalc.Exceptions;
using SharpCalc.Operators;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace SharpCalc.Components;

public class Equator : IEqualityComparer<IMathNode>
{
    public readonly static IEqualityComparer<IMathNode> Instance = new Equator();
    static public bool Equals(IMathNode left, IMathNode right)
    {
        if (left == null && right == null) return true;
        else if (left == null || right == null) return false;
        else return ReferenceEquals(left, right) || left.Equals(right) || right.Equals(left);       
    }

    bool IEqualityComparer<IMathNode>.Equals(IMathNode? x, IMathNode? y)
    {
        if (x == null || y == null) return ReferenceEquals(x, y);
        else return Equals(x, y);
    }

    public int GetHashCode([DisallowNull] IMathNode obj)
    {
        throw new YouShouldntBeHere();
    }
    private Equator()
    {
    }
}
