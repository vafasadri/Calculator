using SharpCalc.Operators;

namespace SharpCalc
{
    internal abstract record class OperatorMetadata
    {        
        public const int MinPrecedence = -2;
        public const int MaxPrecedence = 5;
        private static long IdGen = 0;
        public long Id { get; }
        public string Name { get; }
        public bool IsEquational { get; }
        public int Precedence { get; }

        public OperatorMetadata(string name, int precdence,bool isEquation)
        {
            Id = IdGen++;
            Name = name;
            Precedence = precdence;
            IsEquational = isEquation;
        }      
    }
    internal sealed record class OperatorGroupMetadata : OperatorMetadata
    {
        public delegate IOperatorGroup Creator();
        private Creator _emptyCreator;
        public bool IsLeftOptional { get; }
        //internal bool IsRightOptional { get; init; }
        private OperatorGroupMetadata(string name, int prec, Creator creator, bool leftoptional) : base(name, prec,false)
        {
            _emptyCreator = creator;
            IsLeftOptional = leftoptional;
        }
        public IOperatorGroup CreateInstance()
        {
            return _emptyCreator();
        }
        public static OperatorGroupMetadata Register(string name, int prec, Creator creator, bool leftoptional = false)
        {
            return new OperatorGroupMetadata(name, prec, creator, leftoptional);
        }
    }
    internal sealed record class SingleOperatorMetadata : OperatorMetadata
    {
        public delegate IOperator Creator(Word? left, Word? right);
        private Creator _creator;
        internal bool AssociatesLeft { get; }
        internal bool AssociatesRight { get; }
        public IOperator CreateInstance(Word? leftOperand, Word? rightOperand)
        {
            if (AssociatesLeft && leftOperand == null) throw new Exception();
            if (AssociatesRight && rightOperand == null) throw new Exception();
            return _creator(leftOperand, rightOperand);
        }
        private SingleOperatorMetadata(string name, int prec, Creator creator, bool assocleft, bool assocright,bool isEquational) : base(name, prec,isEquational)
        {
            _creator = creator;
            AssociatesLeft = assocleft;
            AssociatesRight = assocright;
        }
        public static SingleOperatorMetadata Register(string name, int prec, Creator creator, bool assocleft = true, bool assocright = true,bool isEquational = false) 
        {
            return new SingleOperatorMetadata(name, prec, creator, assocleft, assocright,isEquational);
        }
    }
}
