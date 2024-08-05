using SharpCalc.DataModels;
using System.Diagnostics;
using System.Collections;
using System.ComponentModel;

namespace SharpCalc.Components;

public static class Utilities
{

    internal static IEnumerable<LinkedList<object>> SplitByComma(LinkedList<object> p,bool temporary = true)
    {       
        if (p.Count == 0) yield break;
        LinkedList<object> tempCont = new();        
        for (var i = p.First; i != null; i = i.Next)
        {
            if (i.Value is Symbol.Comma)
            {
                yield return tempCont;
                if (temporary) tempCont.Clear();
                else temporary = new();
            }
            else tempCont.AddLast(i.Value);
        }
    }
    [DebuggerStepThrough]
    internal static bool SimplifyAny(IEnumerable<IMathNode> nodes,in IList<IMathNode?> output)
    {        
        bool any = false;      
        int index = 0;
        foreach (var node in nodes)
        {           
            output[index++] = node.Simplify(out bool did);
            any  = any || did;
        }
        return any;
    }
    public static bool TryConvertToNumber(this IMathNode? node, out Complex o)
    {
        if(node is Number n)
        {

            o = n.Value;
            return true;
        }
        else
        {
            o = new();
            return false;
        }     
    }
    public static IMathNode WrapNumber(this double val)
    {
        return new Number(val);
    }
    public static bool IsNumberRegular(Complex number)
    {
        return number.a == 0 && number.b >= 0;
    }   
}
struct SingleValueList<T> : IReadOnlyList<T>
{
    struct Enumerator : IEnumerator<T>
    {
        readonly T value;
        bool showed;

        public T Current => showed ? value : throw new Exception();

        object IEnumerator.Current => showed ? value : throw new Exception();

        public void Dispose() { }
        public bool MoveNext()
        {
            if (showed) return false;
            showed = true;
            return true;
        }
        public void Reset() => showed = false;
        public Enumerator(T value)
        {
            this.value = value;
        }
    }
    T value;
    public T this[int index]
    {
        get
        {
            if (index != 0) throw new ArgumentOutOfRangeException(nameof(index));
            return value;
        }
    }

    public int Count => 1;

    public IEnumerator<T> GetEnumerator() => new Enumerator(value);
    IEnumerator IEnumerable.GetEnumerator() => new Enumerator(value);
    public SingleValueList(T value)
    {
        this.value = value;
    }
}