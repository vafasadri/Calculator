using SharpCalc.Operators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SharpCalc.DataModels;
internal interface ISimplification
{
    Type LeftOperandType { get; }
    Type RightOperandType { get; }
    Type OperationBetween { get; }
    IMathNode? Simplify(IMathNode? left, IMathNode? right);
}
internal interface ISimplification<TLeft, TRight, TOp> : ISimplification
where TLeft : IMathNode where TRight : IMathNode where TOp : Operators.OperatorBase
{
    Type ISimplification.LeftOperandType => typeof(TLeft);
    Type ISimplification.RightOperandType => typeof(TRight);
    Type ISimplification.OperationBetween => typeof(TOp);
    IMathNode? Simplify(TLeft left, TRight right);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    IMathNode? ISimplification.Simplify(IMathNode? left, IMathNode? right)
    {
        return Simplify((TLeft)left, (TRight)right);
    }
}
