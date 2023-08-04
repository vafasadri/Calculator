using SharpCalc.DataModels;

namespace SharpCalc.Operators.Arithmetic
{
    internal class Power : IOperator, IArithmeticOperator
    {
        internal Word Base;
        internal Word Exponent;
        public static readonly SingleOperatorMetadata Metadata = SingleOperatorMetadata.Register
        (
            "Power", 0,
            (left, right) => new Power(left, right)
        );
        public Power(Word left, Word right)
        {
            this.Base = left;
            this.Exponent = right;
        }
        OperatorMetadata IOperator.Metadata => Metadata;
        public string ToText()
        {
            return $"{this.OpAwarePrint(Base)} ^ {this.OpAwarePrint(Exponent)}";
        }

        public Word? Simplify()
        {
            var l = Base.SuperSimplify(out bool simleft);
            var r = Exponent.SuperSimplify(out bool simright);           
            if (r is Number rn)
            {
                if (l is Number ln) return new Number(Math.Pow(ln.Value, rn.Value));
                else if (rn.Value == 0) return new Number(1);
                else if (rn.Value == 1) return l;
            }
            if(l is Number ln2)
            {

                if (r is Multiply mr && mr.NumberFactor != 1) {

                    return new Power(new Number(Math.Pow(ln2.Value, mr.NumberFactor)), new Multiply(mr.Factors!));
                        }
                else if(r is Add ar && ar.NumberFactor != 0)
                {
                    return new Multiply(new Number(Math.Pow(ln2.Value,ar.NumberFactor)), new Power(ln2,new Add(ar.Factors!)));
                }
            }
            else if(l is Multiply && r is Number rn2 &&  rn2.Value == Math.Floor(rn2.Value))
            {
                return new Multiply(Enumerable.Repeat(l, (int)rn2.Value));
            }
            else if(l is Power power)
            {
                return new Power(power.Base, new Multiply(power.Exponent, r));
            }
            if (simleft || simright)
            {
                return new Power(l, r);
            }
            else return null;
        }

        void Word.FindX(VariableLocator locator)
        {
            locator.Path.Push(this);
            Base.FindX(locator);
            if (locator.variable != null) return;
            Exponent.FindX(locator);
            if (locator.variable == null) locator.Path.Pop();
        }

        Word IContent.Derivative(Proxy x)
        {

            throw new NotImplementedException();
        }
    }
}
