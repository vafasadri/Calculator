using SharpCalc.DataModels;
using SharpCalc.Exceptions;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace SharpCalc.Components;
/// <summary>
/// Contains methods to convert a string to a linked list of objects the app can understand
/// <br/>
///  like (2 + 2) * 3 -> LinkedList{ LinkedList{2,Symbol.Plus,2}, Symbol.Cross, 3} <br/>
///  which will later be converted to operator classes like new Multiply(new Add(new Number(2),new Number(2)),new Number(3))
///
/// </summary>
static public class LexicalAnalyzer
{
    // Translations:
    // {&} => Abstract
    // {$} => Function
    // {#} => Number
    // {*} => Any
    public enum Extract
    {
        Any, Abstract, Number
    }
    private enum CharType
    {
        WhiteSpace, Digit, Letter, Symbol
    }
    static CharType GetCharType(string str, int index)
    {
        char current = str[index];
        if (char.IsWhiteSpace(current)) return CharType.WhiteSpace;
        else if (char.IsDigit(current)) return CharType.Digit;
        else if (char.IsLetter(current) || current == '_' || current == '$' || current == '#') return CharType.Letter;
        else if (current == '.')
        {
            if (index > 0 && char.IsDigit(str[index - 1]) &&
              index < str.Length - 1 && char.IsDigit(str[index + 1])) return CharType.Digit;
            else return CharType.Symbol;
        }
        else if (char.IsSymbol(current) || char.IsPunctuation(current)) return CharType.Symbol;
        else throw new UnknownSymbolError(current.ToString());
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="layers"></param>
    /// <param name="s"></param>
    /// <param name="type">type of the current word</param>
    /// <param name="allowExtractor">whether allow extractor classes or not</param>
    /// <returns>bytes moved</returns>
    /// <exception cref="Exception"></exception>
    /// <exception cref="UnknownSymbolError"></exception>
    /// <exception cref="UnexpectedClosingSignError"></exception>
    private static int CreateToken(Stack<LinkedList<object>> layers, StringSegment s, CharType type, bool allowExtractor)
    {
        var current = layers.Peek();

        switch (type)
        {
            case CharType.WhiteSpace:
                return s.Length;
            case CharType.Digit:
                current.AddLast(double.Parse(s.ToString()));
                return s.Length;
            case CharType.Letter:
                current.AddLast(s.ToString());
                return s.Length;
            case CharType.Symbol:
                {
                    switch (s[0])
                    {
                        case '{':
                            if (!allowExtractor) throw new ExtractorsNotAllowedException();
                            if (s.Length < 3) throw new InvalidExtractorException();
                            Extract code = s[1] switch
                            {
                                '&' => Extract.Abstract,
                                '*' => Extract.Any,
                                '#' => Extract.Number,
                                _ => throw new InvalidExtractorCodeException(s[1])
                            };
                            current.AddLast(code);
                            if (s[2] != '}') throw new UnexpectedClosingSignError();
                            return 3;
                        case '(':
                            LinkedList<object> n = new();
                            current.AddLast(n);
                            layers.Push(n);
                            return 1;
                        case ')':
                            if (layers.Count <= 1)
                                throw new UnexpectedClosingSignError();
                            else layers.Pop();
                            return 1;
                        default:
                            Symbol symbol = SymbolIO.Get(s, out int move);
                            current.AddLast(symbol);
                            return move;
                    }
                }
            default:
                return 0;
        }
    }

    static public LinkedList<object> ToLexicalSeries(string m, bool allowExtractors)
    {
        LinkedList<object> xbase = new();
        Stack<LinkedList<object>> layers = new();
        layers.Push(xbase);
        // points to the first character of the current slice
        int tokeStart = 0;
        // type of the last checked character, starting with the first character       
        CharType lastType = m.Length > 0 ? GetCharType(m, 0) : CharType.WhiteSpace;
        void fillTill(int end)
        {
            while (tokeStart < end)
            {
                tokeStart += CreateToken(layers, new StringSegment(m, tokeStart, end), lastType, allowExtractors);
            }
        }
        for (int i = 0; i < m.Length; i++)
        {
            CharType currentType = GetCharType(m, i);
            if (lastType != currentType)
            {
                fillTill(i);
                Debug.Assert(tokeStart == i);
            }
            lastType = currentType;
        }
        fillTill(m.Length);
        //PushOperators(xbase.ByteCode);
        return xbase;
    }

    static bool Is(object? f, object v)
    {
        return f switch
        {
            Extract.Any => v is not null,
            Extract.Abstract => v is string,
            Extract.Number => v is Number,
            LinkedList<object> => v is LinkedList<object>,
            null => false,
            _ => Equals(f, v)
        };
    }
    /// <summary>
    /// checks if an expression matches a syntax expression and fills any extractors
    /// </summary>
    /// <param name="expression">the expression</param>
    /// <param name="syntax">syntax to match</param>
    /// <param name="container">container to add the extracted objects</param>
    /// <returns>if the expression and syntax match</returns>
    public static bool MatchSyntax(in LinkedList<object> expression, in LinkedList<object> syntax, in List<object> container)
    {
        LinkedList<object>? tempexp = null;
        var sNode = syntax.First;
        var xNode = expression.First;
        for (; sNode != null && xNode != null; xNode = xNode.Next)
        {
            if (!Is(sNode.Value, xNode.Value))
            {
                return false;
            }
            if (sNode.Value is LinkedList<object> innerS)
            {
                if (!MatchSyntax((LinkedList<object>)xNode.Value, innerS, container)) return false;
            }
            else if (sNode.Value is Extract f)
            {
                if (f == Extract.Any)
                {
                    tempexp ??= new();
                    tempexp.AddLast(xNode.Value);
                    // is it ending?
                    if (sNode.Next?.Value is Extract.Any)
                        throw new ArgumentException("there are two or more 'any' extractors in a row");
                    if (xNode.Next == null || Is(sNode.Next?.Value, xNode.Value))
                    {
                        container.Add(tempexp);
                        tempexp = null;
                    }
                    else continue;
                }
                else container.Add(xNode.Value);
            }
            sNode = sNode.Next;
        }
        // whether we've reached the end of the syntax or not
        return sNode == null;
    }

}

