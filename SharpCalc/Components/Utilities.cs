using SharpCalc.DataModels;
using System.Diagnostics;
namespace SharpCalc.Components;

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
    internal static Real SuperSimplify(this Real m, out bool did)
    {       
        Real output = m;
        did = false;
        Real? newOutput;
        while ((newOutput = output.Simplify()) != null)
        {
            did = true;
            output = newOutput;
        }
        return output;
    }   
    internal static IMathNode TrySimplify(this IMathNode m,out bool did)
    {
        did = false;
        return (m as Real)?.SuperSimplify(out did) ?? m;
    }
    internal static bool SimplifyAny(IEnumerable<IMathNode> words, out IMathNode[] output)
    {
        Debug.Assert(words.Any());
        bool any = false;
        output = new IMathNode[words.Count()];
        int index = 0;
        foreach (var word in words)
        {           
            output[index++] = TrySimplify(word,out bool did);
            any  = any || did;           
        }
        return any;
    }
    public static bool TryConvertToNumber(this IMathNode? word, out Complex o)
    {
        o = new(0.0);
        switch (word)
        {
            case Number n:
                o = n.Value;
                return true;
            case Variable variable:              
                o = variable.Equation?.NumberValue ?? new Complex(0);
                return variable.Equation?.NumberValue != null;
            case IValued valued:
                return valued.Value.TryConvertToNumber(out o);
            default:
                return false;
        }
    }
    public static Real ToWord(this double val)
    {
        return new Number(val);
    }
    public static bool IsNumberRegular(Complex number)
    {
        return number.a == 0 && number.b >= 0;
    }   
}