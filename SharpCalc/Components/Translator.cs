using SharpCalc.DataModels;
using SharpCalc.Operators;
using SharpCalc.Operators.Arithmetic;
using System.Diagnostics;

namespace SharpCalc.Components;
public static class Classifier
{
    /// <summary>
    /// a list of math symbols like + or - and their associated Operator object metadata
    /// </summary>
    static readonly SortedDictionary<Symbol, OperatorMetadata> Infos = new()
        {
            {Symbol.Plus,Add.MetadataValue },
            {Symbol.Minus,Add.MetadataValue },
            {Symbol.Power,Power.MetadataValue },
            {Symbol.Comma, Operators.Tuple.MetadataValue },
            {Symbol.Cross,Multiply.MetadataValue },
            {Symbol.Assign,Equations.Equation.MetadataValue},
            {Symbol.Invisible_FunctionCall,FunctionCall.MetadataValue },
            {Symbol.Point,Multiply.MetadataValue },
            {Symbol.Slash,Multiply.MetadataValue },
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
        var left = node.Previous?.Value as IMathNode;
        var right = node.Next?.Value as IMathNode;
        if (info is OperatorGroupMetadata groupInfo)
        {
            // add an operand to the previous operator
            if (left is OperatorGroupBase{IsSealed:false } opGroup && opGroup.Metadata == groupInfo)
            {
                expression.Remove(node.Previous!);
                expression.Remove(node.Next!);
                opGroup.AddOperand(opGroup.Convert(right!, opSymbol));
                node.Value = opGroup;
            }
            // create a new operator
            else
            {
                var multi = groupInfo.CreateInstance();
                if (!groupInfo.IsLeftOptional || left != null)
                {
                    if (left is OperatorGroupBase lmulti) lmulti.Seal();
                    expression.Remove(node.Previous!);
                    multi.AddOperand(left!);
                }
                if (right is OperatorGroupBase rmulti) rmulti.Seal();
                multi.AddOperand(multi.Convert(right!, opSymbol));
                expression.Remove(node.Next!);
                node.Value = multi;
            }
        }
        else if (info is SingleOperatorMetadata singleInfo)
        {
            if (singleInfo.AssociatesLeft)
            {
                if (left is OperatorGroupBase lmulti) lmulti.Seal();
                expression.Remove(node.Previous!);
            }
            if (singleInfo.AssociatesRight)
            {
                if (right is OperatorGroupBase rmulti) rmulti.Seal();
                expression.Remove(node.Next!);
            }
            node.Value = singleInfo.CreateInstance(left, right);
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
    /// converts series of lexical tokens into a <see cref="IMathNode"/>
    /// </summary>    
    /// <param name="data">the data bank to search for names</param>
    /// <param name="parameters">a list of function parameters to replace occurences of its name with, used when classifying a function body</param>
    internal static IMathNode Classify(LinkedList<object> series, DataBank? data, IEnumerable<RuntimeFunction.Parameter>? parameters)
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
        if (series.First.Value is OperatorGroupBase opg) opg.Seal();
        return (IMathNode)series.First.Value;
    }

    public static IMathNode SolveExternal(LinkedList<object> x)
    {
        var s = Classify(x, null, null);
        return s.TrySimplify(out _);
    }
}

