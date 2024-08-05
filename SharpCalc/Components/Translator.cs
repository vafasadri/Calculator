using SharpCalc.DataModels;
using SharpCalc.Equations;
using SharpCalc.Operators;
using SharpCalc.Operators.Arithmetic;
using SharpCalc.Operators.Logic;
using System.Diagnostics;

namespace SharpCalc.Components;
public static class Classifier
{
    /// <summary>
    /// a list of math symbols like + or - and their associated Operator object metadata
    /// </summary>
    static readonly SortedDictionary<Symbol, OperatorMetadata> Infos = new()
        {
            {Symbol.Plus,Add.Metadata },
            {Symbol.Minus,Add.Metadata },
            {Symbol.Power,Power.Metadata },
            {Symbol.Comma, Operators.Tuple.Metadata },
            {Symbol.Cross,Multiply.Metadata },
            {Symbol.Assign, Equations.Equal.Metadata},
            {Symbol.Invisible_FunctionCall,FunctionCall.Metadata },
            {Symbol.Point,Multiply.Metadata },
            {Symbol.Slash,Multiply.Metadata },
            {Symbol.Tilde,Not.Metadata },
            {Symbol.Ampersand,And.Metadata },
            {Symbol.Pipe,Or.Metadata },
            {Symbol.Implies,Condition.Metadata},
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
            if (left is OperatorGroupBase { IsSealed: false } opGroup && opGroup.Metadata == groupInfo)
            {
                expression.Remove(node.Previous!);
                expression.Remove(node.Next!);
                if (right is OperatorGroupBase opg) opg.Seal();
                opGroup.AddOperand(opGroup.Convert(right!, opSymbol));

                node.Value = opGroup;
            }
            // create a new operator
            else
            {
                if (node.Next == null || right == null) throw new Exceptions.ExpectingError("an operand", $"after operator '{info.Name}'", right);
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

    static IDataModel? GetDataInternal(string str, DataBank? bank, IEnumerable<Variable>? parameters, bool force)
    {
        IDataModel? a = parameters?.SingleOrDefault(s => s.Name == str);
        IReadonlyDataBank readonlybank = bank ?? StaticDataBank.DataBank;
        a ??= readonlybank!.GetData(str);
        if (force)
        {
            a ??= bank!.ForceGetData(new ShallowName(str));
        }
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
    static IDataModel GetData(string str, DataBank? bank, IEnumerable<Variable>? parameters)
    {
        // all of this only to add differential notations
        // stupid!
        IDataModel? d = null;

        int primes = 0;
        int ds = 0;
        while (str.EndsWith('\'') || str.EndsWith('"') || str.EndsWith('`'))
        {
            if (str.EndsWith('\'')) primes++;
            else if (str.EndsWith('"')) primes += 2;
            else if (str.EndsWith('`')) primes++;
            str = str.Remove(str.Length - 1);
        }
        while (str.StartsWith('d') && (d = GetDataInternal(str, bank, parameters, false)) == null)
        {
            ds++;
            str = str.Remove(0, 1);
        }

         d ??= GetDataInternal(str, bank, parameters, true);


        if (d is IFunction func)
        {
            if (ds != 0) throw new Exceptions.CustomError("Cannot use d with functions, use primes (',`,\") instead");
            for (int i = 0; i < primes; i++)
            {
                func = func.GetDerivative();
            }
            return func;
        }
        else if (d is Variable proxy)
        {
            if (primes != 0) throw new Exceptions.CustomError("Cannot use prime with variables, use d instead");
            for (int i = 0; i < ds; i++)
            {
                proxy = proxy.Differential;
            }
            return proxy;
        }

        return d!;
    }
    /// <summary>
    /// converts series of lexical tokens into an <see cref="IMathNode"/>
    /// </summary>    
    /// <param name="data">the data bank to search for names</param>
    /// <param name="parameters">a list of function parameters to replace occurences of its name with, used when classifying a function body</param>
    internal static IMathNode Classify(LinkedList<object> series, DataBank? data, IEnumerable<Variable>? parameters)
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
            CombineNode(series, top);
        }
        Debug.Assert(series.Count <= 1);
        if (series.Count == 0) return Operators.Tuple.Empty;
        if (series.First!.Value is OperatorGroupBase opg) opg.Seal();
        return (IMathNode)series.First.Value;
    }

    public static IMathNode SolveExternal(LinkedList<object> x)
    {
        var s = Classify(x, null, null);
        return s.Simplify(out _);
    }
}

