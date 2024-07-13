using SharpCalc.Exceptions;
using SharpCalc.Operators;
using System.Diagnostics;

namespace SharpCalc.DataModels
{
    internal abstract record class OperatorMetadata (
        string Name,
        int Precedence,
        IEnumerable<ISimplification> Simplifications,
        bool IsEquational)
    {       
        private static long IdGen = 0;
        public long Id { get; } = IdGen++;
        public IEnumerable<ISimplification> Simplifiacations { get; } = Simplifications.ToArray();                    
    }
    internal delegate OperatorGroupBase GroupCreator();
    internal delegate OperatorGroupBase FullGroupCreator(IEnumerable<IMathNode> factors);
    internal sealed record class OperatorGroupMetadata(string Name,
        int Precedence,
        GroupCreator EmptyCreator,
        FullGroupCreator FullCreator,
        IEnumerable<ISimplification> Simplifications,
        Real EmptyValue,
        Number NumberFactorDefault,
        bool IsLeftOptional = false
        ) : OperatorMetadata(Name, Precedence, Simplifications, false)
    {           
        public OperatorGroupBase CreateInstance()
        {
            return EmptyCreator();
        }
        public OperatorGroupBase CreateInstance(IEnumerable<IMathNode> factors)
        {
            var returnValue = FullCreator(factors);
            Debug.Assert(returnValue.IsSealed);
            return returnValue;
        }     
    }
    internal delegate SingleOperatorBase SingleCreator(IMathNode? left, IMathNode? right);
    internal sealed record class SingleOperatorMetadata(
        string Name,
        int Precedence,
        SingleCreator Creator,
        IEnumerable<ISimplification> Simplifications,
        bool AssociatesLeft = true,
        bool OperandsSwappable = false,
        bool AssociatesRight = true,
        bool IsEquational = false)
        : OperatorMetadata(Name, Precedence,Simplifications,IsEquational)
    {
        private readonly SingleCreator Creator = Creator;
        public SingleOperatorBase CreateInstance(IMathNode? leftOperand, IMathNode? rightOperand)
        {
            if ((AssociatesLeft && leftOperand == null) ||
            (AssociatesRight && rightOperand == null)) throw new ExpectingError("an operand", $"in operator {Name}", null);
            return Creator(leftOperand, rightOperand);
        }
    }
}
