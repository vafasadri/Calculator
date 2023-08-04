using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
namespace SharpCalc;
/// <summary>
/// used instead of <see cref="string.Substring(int, int)"/> to avoid unneccesary character copying
/// </summary>
public struct StringSegment : IComparable<StringSegment>
{
    public string Value;
    int Start;
    int End;
    public char this[int index]
    {
        get
        {
            if (index + Start > End) throw new ArgumentOutOfRangeException(nameof(index));
            return Value[index + Start];
        }
    }
    public int Length => Math.Min(Value.Length, End) - Start;
    public StringSegment(string val, int start, int end)
    {
        this.Value = val;
        this.Start = start;
        this.End = end;
    }
    public StringSegment Sub(int startindex, int length)
    {
        return new(Value, Start + startindex, Start + startindex + length);
    }
    public override string ToString()
    {
        return Value[Start..End];
    }

    public int CompareTo(StringSegment other)
    {     
        var compareIntersect = string.Compare(this.Value, this.Start, other.Value, other.Start,Math.Min(this.Length,other.Length));

        if (compareIntersect == 0)
        {
            return this.Length.CompareTo(other.Length);
        }
        else return compareIntersect;
    }

    public StringSegment(string val)
    {
        Value = val;
        Start = 0;
        End = val.Length;
    }
    public static implicit operator StringSegment(string str)
    {
        return new StringSegment(str);
    }
}
public enum Symbol
{
    Null, Plus, Minus, Cross, Slash, Equal, Assign, Greater, GreaterOrEqual, Smaller, SmallerOrEqual, NonEqual,  Power, Comma, Point, ExclamationMark,
    Invisible_FunctionCall
}
public static class SymbolIO
{
    private static readonly string[] Out =
    {
        String.Empty,"+","-","*","/","==","=",">",">=","<","<=","!=","^",",",".","!",String.Empty
    };

    private static readonly SortedDictionary<StringSegment,Symbol> In = new()
    {
        { "+",Symbol.Plus },
        { "-",Symbol.Minus },
        { "*",Symbol.Cross },
        { "/",Symbol.Slash },
        { "\\",Symbol.Slash },
        { "^",Symbol.Power },
        { ".",Symbol.Point },
        { ",",Symbol.Comma },
        { "==",Symbol.Equal },
        { "!=",Symbol.NonEqual },
        { "=!",Symbol.NonEqual },
        { "<=",Symbol.SmallerOrEqual },
        { "=<",Symbol.SmallerOrEqual },
        { ">=",Symbol.GreaterOrEqual },
        { "=>",Symbol.GreaterOrEqual },
        { "=",Symbol.Assign },
        { ">",Symbol.Greater },
        { "<",Symbol.Smaller },
        { "!",Symbol.ExclamationMark },
    };
    public static string Represent(Symbol s)
    {
        return Out[(int)s];
    }
    internal static Symbol Get(StringSegment str, out int MoveSize)
    {
        Symbol result;
        if (str.Length >= 2)
        {
            var s2 = str.Sub(0, 2);
            result = In.GetValueOrDefault(s2);
            if (result != Symbol.Null)
            {
                MoveSize = 2;
                return result;
            }
        }
        var s1 = str.Sub(0, 1);
        result = In.GetValueOrDefault(s1);
        if (result != Symbol.Null)
        {
            MoveSize = 1;
            return result;
        }
        throw new Exceptions.UnknownSymbolError(str.ToString());
    }
}