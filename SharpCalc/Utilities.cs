using SharpCalc.DataModels;
using SharpCalc.Operators;
using System.Diagnostics;
namespace SharpCalc;

public static class Utilities
{
    internal static List<LinkedList<object>> SplitByComma(LinkedList<object> p)
    {
        List<LinkedList<object>> split = new();
        LinkedList<object> tempCont = new();
        for (var i = p.First; i != null; i = i.Next)
        {
            if (i.Value is Symbol.Comma)
            {
                split.Add(tempCont);
                tempCont = new();
            }
            else tempCont.AddLast(i.Value);
        }
        split.Add(tempCont);
        return split;
    }
    internal static Word SuperSimplify(this Word m, out bool did)
    {
        Word output = m;
        did = false;
        Word? newOutput;
        while ((newOutput = output.Simplify()) != null)
        {
            did = true;
            output = newOutput;
        }
        return output;
    }
    internal static bool SimplifyAny(IEnumerable<Word> words, out Word[] output)
    {
        Debug.Assert(words.Any());
        bool any = false;
        output = new Word[words.Count()];
        int index = 0;
        foreach (var word in words)
        {
            output[index++] = word.SuperSimplify(out bool did);
            if (did) any = true;
        }
        return any;
    }
    internal static Number ToWord(this double num)
    {
        return new Number(num);
    }
    internal static string OpAwarePrint(this IOperator m, Word w)
    {
        if (w is IOperator o && o.Metadata.Precedence >= m.Metadata.Precedence)
            return $"({w.ToText()})";
        else return w.ToText();
    }

    internal static List<T>? Separate<T>(IEnumerable<Word> words, Action<double> onNumberFound, Converter<Word, T> converter)
    {
        Debug.Assert(words.Any());
        List<T>? cont = null;
        foreach (var item in words)
        {
            if (item is Number num)
            {
                onNumberFound(num.Value);
            }
            else
            {
                cont ??= new();
                cont.Add(converter(item));
            }
        }
        cont?.TrimExcess();
        return cont;
    }
    static internal void IterateFactors(VariableLocator locator, IEnumerable<Word>? words)
    {
        Debug.Assert(locator.variable == null);

        if (words == null) return;
        foreach (var item in words)
        {
            item.FindX(locator);
            if (locator.variable != null) return;
        }
    }
    public static bool TryConvertToNumber(this Word? word, out double o)
    {
        o = 0.0;
        switch (word)
        {
            case Number n:
                o = n.Value;
                return true;
            case Variable variable:
                double? f = variable.Equation?.NumberValue;
                o = f ?? 0.0;
                return f != null;
            case IValued valued:
                return TryConvertToNumber(valued.Value, out o);
            default:
                return false;
        }
    }
}