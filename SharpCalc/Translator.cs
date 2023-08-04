using SharpCalc.DataModels;
using SharpCalc.Operators;
using SharpCalc.Operators.Arithmetic;
using System.Collections.Specialized;
using System.Diagnostics;

namespace SharpCalc;
public class Translator
{
    /// <summary>
    /// a list of math symbols like + or - and their associated Operator object constructors
    /// </summary>
    static readonly SortedDictionary<Symbol, OperatorMetadata> Infos = new()
        {
            {Symbol.Plus,Add.Metadata },
            {Symbol.Minus,Add.Metadata },
            {Symbol.Power,Power.Metadata },
            {Symbol.Comma, Operators.Tuple.Metadata },
            {Symbol.Cross,Multiply.Metadata },
            {Symbol.Assign,Equation.Metadata },
            {Symbol.Invisible_FunctionCall,FunctionCall.Metadata },
            {Symbol.Point,Multiply.Metadata },
            {Symbol.Slash,Multiply.Metadata },
        };

    /// converts the symbol on top of the stack into an operator
    /// </summary>
    /// <param name="expression"></param>
    /// <param name="nodes"></param>
    static void CombineNode(LinkedList<object> expression, LinkedListNode<object> node)
    {
        // node containing the symbol       
        var opSymbol = (Symbol)node.Value;
        var info = Infos[opSymbol];
        var left = (Word?)node.Previous?.Value;
        var right = (Word?)node.Next?.Value;
        if (left is IOperatorGroup mr && mr.Metadata == info)
        {
            expression.Remove(node.Previous!);
            expression.Remove(node.Next!);
            mr.AddOperand(mr.Convert(right!, opSymbol));
            node.Value = mr;
        }
        else if (info is OperatorGroupMetadata minfo)
        {
            var multi = minfo.CreateInstance();
            if (!(minfo.IsLeftOptional && left == null))
            {
                if (left is IOperatorGroup lmulti) lmulti.Seal();
                expression.Remove(node.Previous!);
                multi.AddOperand(left!);
            }
            if (right is IOperatorGroup rmulti) rmulti.Seal();
            multi.AddOperand(multi.Convert(right!, opSymbol));
            expression.Remove(node.Next!);
            node.Value = multi;
        }
        else if (info is SingleOperatorMetadata sInfo)
        {
            if (sInfo.AssociatesLeft)
            {
                if (left is IOperatorGroup lmulti) lmulti.Seal();
                expression.Remove(node.Previous!);
            }
            if (sInfo.AssociatesRight)
            {
                if (right is IOperatorGroup rmulti) rmulti.Seal();
                expression.Remove(node.Next!);
            }
            node.Value = sInfo.CreateInstance(left, right);
        }
    }

    static IDataModel GetData(string str, DataBank? bank, IEnumerable<RuntimeFunction.Parameter>? parameters)
    {
        IDataModel? a = parameters?.SingleOrDefault(s => s.Name == str);
        a ??= bank!.ForceGetData(new ShallowName(str));
        return a;
    }

    static bool GetTopInfo(Stack<LinkedListNode<object>> stack, out OperatorMetadata info)
    {
        if (stack.TryPeek(out var lastNode))
        {
            info = Infos[(Symbol)lastNode.Value];
            return true;
        }
        info = null;
        return false;
    }
    /// <summary>
    /// converts series of objects into a <see cref="Word"/>
    /// </summary>    
    /// <param name="data">the data bank to search for variables</param>
    /// <param name="parameters">a list of function parameters to replace occurences of its name with, used when classifying a function body</param>
    internal static Word Classify(LinkedList<object> series, DataBank? data, IEnumerable<RuntimeFunction.Parameter>? parameters)
    {
        Stack<LinkedListNode<object>> opStack = new();
        for (var node = series.First; node != null; node = node.Next)
        {
            switch (node.Value)
            {
                case string str:
                    node.Value = GetData(str, data, parameters);
                    // code for detecting function calls
                    if (node.Value is IFunction && node.Next?.Value is not Symbol and not null)
                    {
                        series.AddAfter(node, Symbol.Invisible_FunctionCall);
                    }
                    break;
                case IFunction:
                    if (node.Next?.Value is not Symbol and not null)
                    {
                        series.AddAfter(node, Symbol.Invisible_FunctionCall);
                    }
                    break;
                case double n:

                    node.Value = new Number(n);
                    break;
                case LinkedList<object> list:
                    node.Value = Classify(list, data, parameters);
                    break;
                case Symbol symbol:
                    var thisInfo = Infos[symbol];
                    while (GetTopInfo(opStack, out var lastInfo) &&
                   (lastInfo != thisInfo || lastInfo is OperatorGroupMetadata) &&
                   lastInfo.Precedence <= thisInfo.Precedence)
                    {
                        if (lastInfo.IsEquational) throw new Exceptions.EquationChildError();
                        CombineNode(series, opStack.Pop());
                    }
                    opStack.Push(node);
                    break;
            }
            if (node.Value is not Symbol && node.Next?.Value is not Symbol and not null)
            {
                series.AddAfter(node, new LinkedListNode<object>(Symbol.Point));
            }
        }

        while (opStack.Count > 0)
        {
            var top = opStack.Pop();
            if (Infos[(Symbol)top.Value].IsEquational && opStack.Count > 0) throw new Exceptions.EquationChildError();
            CombineNode(series, top);
        }
        Debug.Assert(series.Count == 1);     
        return (Word)series.First.Value;
    }

    public static Word SolveExternal(LinkedList<object> x)
    {
        return Classify(x, null, null).SuperSimplify(out _);
    }
}

