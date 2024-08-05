using SharpCalc.Exceptions;
using SharpCalc.Operators;
using System.Collections.Specialized;
using System.Diagnostics;

namespace SharpCalc.DataModels
{
    internal abstract record class OperatorMetadata
    {
        static readonly Dictionary<Type, OperatorMetadata> List = new();
        private static long IdGen = 0;
        public long Id { get; } = IdGen++;
        public required string Name { get; init; }
        public required int Precedence { get; init; }
        public abstract bool OperandsSwappable { get; init; }
        public virtual bool AssociatesLeft { get; init; }
        public virtual bool AssociatesRight { get; init; }
        public IReadOnlyList<ISimplification> Simplifications { get; }
        public Type Operator { get; }

        private List<(ListDictionary left, ListDictionary right)> TypeCache;
        private readonly Dictionary<(Type?, Type?), (ISimplification, bool swapped)> PairCache;

        bool MatchesLeft(ISimplification simplification, (ListDictionary left, ListDictionary right) slot, Type? type)
        {
            if (!AssociatesLeft ^ type == null) throw new CustomError("wtf");
            if (!AssociatesLeft && type == null) return true;
            var cache = slot.left;
            var z = cache[type];
            if (z != null)
            {
                return (bool)z;
            }
            bool a = simplification.LeftOperandType.IsAssignableFrom(type);
            cache[type] = a;
            return a;
        }
        bool MatchesRight(ISimplification simplification, (ListDictionary left, ListDictionary right) slot, Type? type)
        {
            if (!AssociatesRight ^ type == null) throw new CustomError("wtf");
            if (!AssociatesRight && type == null) return true;
            var cache = slot.right;
            var z = cache[type];
            if (z != null)
            {
                return (bool)z;
            }
            bool a = simplification.RightOperandType.IsAssignableFrom(type);
            cache[type] = a;
            return a;
        }
        void AddSuperCache(Type? left, Type? right, ISimplification simplification, bool swapped)
        {
            PairCache.TryAdd((left, right), (simplification, swapped));
            if (OperandsSwappable) PairCache.TryAdd((left, right), (simplification, !swapped));
        }
        public IMathNode? TrySimplify(IMathNode left, IMathNode right)
        {
            var leftType = left?.GetType();
            var rightType = right?.GetType();

            if (PairCache.TryGetValue((leftType, rightType), out var pair))
            {
                var left2 = left;
                var right2 = right;
                if (pair.swapped) (left2, right2) = (right, left);
                var ret = pair.Item1.Simplify(left2, right2);
                if (ret != null) return ret;
            }

            for (int i = 0; i < Simplifications.Count; i++)
            {
                var item = Simplifications[i];
                var slot = TypeCache[i];
                if (MatchesLeft(item, slot, leftType) && MatchesRight(item, slot, rightType))
                {
                    var ret = item.Simplify(left, right);
                    if (ret != null)
                    {
                        AddSuperCache(leftType, rightType, item, false);
                        return ret;
                    }
                }
                if (!OperandsSwappable) continue;
                if (MatchesLeft(item, slot, rightType) && MatchesRight(item, slot, leftType))
                {

                    var ret = item.Simplify(right, left);
                    if (ret != null)
                    {
                        AddSuperCache(leftType, rightType, item, true);
                        return ret;
                    }
                }
            }
            return null;
        }
        public static OperatorMetadata GetMetadata(Type Operator)
        {
            return List[Operator];
        }
        public OperatorMetadata(Type Operator, IReadOnlyList<ISimplification> simplifications)
        {
            if (!Operator.IsSubclassOf(typeof(OperatorBase))) throw new Exceptions.CustomError("Type must derive from OperatorBase");
            if (simplifications.Any(n => n.OperationBetween != Operator)) throw new Exceptions.CustomError("All simplifications must have this type as their OperationBetween property");
            this.Operator = Operator;
            PairCache = new();
            Simplifications = simplifications;
            TypeCache = new(simplifications.Count);
            for (int i = 0; i < simplifications.Count; i++)
            {
                TypeCache.Add((new(), new()));
            }
            List.Add(Operator, this);
        }
    }
    internal delegate OperatorGroupBase GroupCreator();
    internal delegate OperatorGroupBase FullGroupCreator(IEnumerable<IMathNode> factors);
    internal sealed record class OperatorGroupMetadata : OperatorMetadata
    {
        public required GroupCreator EmptyCreator { get; init; }
        public required FullGroupCreator FullCreator { get; init; }
        public IMathNode? EmptyValue { get; init; }
        public bool IsLeftOptional { get; init; } = false;
        public bool UnpackSelfType { get; init; } = true;
        public override bool AssociatesLeft { get => true; init => throw new InvalidOperationException(); }
        public override bool AssociatesRight { get => true; init => throw new InvalidOperationException(); }
        public override bool OperandsSwappable { get; init; } = true;
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
        public OperatorGroupMetadata(Type Operator, IReadOnlyList<ISimplification> Simplifications) : base(Operator, Simplifications)
        {
            OperandsSwappable = true;
        }
    }

    internal sealed record class SingleOperatorMetadata : OperatorMetadata
    {
        internal delegate SingleOperatorBase SingleCreator(IMathNode left, IMathNode right);
        public required SingleCreator Creator { private get; init; }
        public override bool AssociatesLeft { get; init; } = true;
        public override bool AssociatesRight { get; init; } = true;
        public override bool OperandsSwappable { get; init; } = false;

        public SingleOperatorBase CreateInstance(IMathNode? leftOperand, IMathNode? rightOperand)
        {
            if ((AssociatesLeft && leftOperand == null) ||
            (AssociatesRight && rightOperand == null)) throw new ExpectingError("an operand", $"in operator {Name}", null);
            return Creator(leftOperand, rightOperand);
        }
        public SingleOperatorMetadata(Type Operator, IReadOnlyList<ISimplification> Simplifications) : base(Operator, Simplifications)
        {

        }
    }
}
