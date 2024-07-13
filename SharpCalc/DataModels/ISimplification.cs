using SharpCalc.Operators;
using SharpCalc.Operators.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCalc.DataModels
{
    internal interface ISimplification
    {
        Type LeftOperandType { get; }
        Type RightOperandType { get; }
        Type OperationBetween { get; }
        IMathNode? Simplify(IMathNode left, IMathNode right);        
    }
    internal interface ISimplification<TLeft, TRight, TOp> : ISimplification 
    where TLeft: IMathNode where TRight : IMathNode where TOp : Operators.OperatorBase
    {
        Type ISimplification.LeftOperandType => typeof(TLeft);
        Type ISimplification.RightOperandType => typeof(TRight);
        Type ISimplification.OperationBetween => typeof(TOp);
        IMathNode? Simplify(TLeft left, TRight right);
        IMathNode? ISimplification.Simplify(IMathNode left, IMathNode right)
        {
            return Simplify((TLeft) left,(TRight) right);
        }
    }
    internal interface IRealSimplification : ISimplification
    {
        Real? Simplify(Real left, Real right);
        IMathNode? ISimplification.Simplify(IMathNode left, IMathNode right)
        {
            return Simplify((Real)left, (Real)right);
        }
    }
    internal interface IRealSimplification<TLeft,TRight,TOp> : IRealSimplification where TLeft : Real where TRight : Real where TOp : OperatorBase
    {
        Type ISimplification.LeftOperandType => typeof(TLeft);
        Type ISimplification.RightOperandType => typeof(TRight);
        Type ISimplification.OperationBetween => typeof(TOp);
        Real? Simplify(TLeft left, TRight right);
        Real? IRealSimplification.Simplify(Real left, Real right)
        {
            return Simplify((TLeft)left, (TRight)right);
        }
    }
}
