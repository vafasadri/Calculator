using SharpCalc.DataModels;
using System.Linq;
using System.Text;

namespace SharpCalc.Operators.Arithmetic
{
    internal class Multiply : IOperatorGroup, IArithmeticOperator
    {
        
        internal double NumberFactor = 1;
        private List<Word>? factors = null;
        public bool IsSealed { get; private set; }
        public IReadOnlyList<Word>? Factors => factors;
        public static readonly OperatorGroupMetadata Metadata = OperatorGroupMetadata.Register(
           "Multiply",2,
           () => new Multiply()
        );
        OperatorGroupMetadata IOperatorGroup.Metadata => Metadata;

        public virtual string ToText()
        {
            if (factors == null) return NumberFactor.ToString();         
            List<Word> up = new();
            Multiply? denominator = null;
            foreach (var item in factors)
            {
                if (item is Power pow && ((pow.Exponent is Number rg && rg.Value < 0) || (pow.Exponent is Multiply mulexp && mulexp.NumberFactor < 0)))
                {
                    denominator ??= new();
                    denominator.AddOperand(new Power(pow.Base, Negative.Create(pow.Exponent).SuperSimplify(out _)));
                }
                else
                {
                    up.Add(item);
                }
            }
            denominator?.Seal();
            if(up.Count == 0 && denominator != null)
            {
                return $"{NumberFactor} / {denominator.ToText()}";
            }
            else
            {
                StringBuilder builder = new();
                if (NumberFactor == -1) builder.Append('-');
                else if (NumberFactor != 1) up.Insert(0, new Number(NumberFactor));

                builder.AppendJoin(" * ", up.Select(n => this.OpAwarePrint(n)));
                if(denominator != null)
                {
                    builder.Append(" / ");
                    builder.Append(denominator.ToText());
                }
                return builder.ToString();
            }
            
        }
        void PowerAwareAdd((Word Base, Word Exponent) item)
        {
            int find = -1;
            if (factors != null) find = factors.FindIndex(n => Equator.Equals(n, item.Base) || (n is Power p && Equator.Equals(p.Base, item.Base)));
            // n or n^y

            if (find == -1)
            {
                factors ??= new();
                if (item.Exponent is Number { Value: 1 })
                {
                    factors.Add(item.Base);
                }
                else factors.Add(new Power(item.Base, item.Exponent));
            }
            else
            {
                if (factors[find] is Power { Exponent: Add { IsSealed: false } d })
                {
                    d.AddOperand(item.Exponent);
                }
                else factors[find] = new Power(item.Base, new Add(item.Exponent, GetPower(factors[find]).Exponent));
            }
        }
        static (Word Base, Word Exponent) GetPower(Word item)
        {
            if (item is Power power)
            {
                return (power.Base, power.Exponent);
            }
            else return (item, new Number(1));
        }
        void AwareAdd(Word item)
        {
            PowerAwareAdd(GetPower(item));
        }
        void AwareRemove(Word item)
        {
            var pow = GetPower(item);
            if (pow.Exponent != null)
                pow.Exponent = Negative.Create(pow.Exponent).SuperSimplify(out _);
            PowerAwareAdd(pow);
        }
        public void AddOperand(Word item)
        {
            if (IsSealed) throw new Exceptions.SealedOperatorException();
            switch (item)
            {
                case Multiply multiply:
                    NumberFactor *= multiply.NumberFactor;
                    foreach (var factor in multiply.factors!)
                    {
                        AwareAdd(factor);
                    }
                    break;
                case Number num:
                    NumberFactor *= num.Value;
                    break;
                default:
                    AwareAdd(item);
                    break;
            }
        }
        public Word? Simplify()
        {
            if (factors == null)
            {
                return new Number(NumberFactor);
            }
            else if (NumberFactor == 1 && factors.Count == 1)
            {
                return factors[0];
            }
            else if (NumberFactor == 0) return new Number(0);
            bool simplified = Utilities.SimplifyAny(factors, out Word[] simpleList);
            //var ad = (Add) simpleList.FirstOrDefault(n => n is Add);
            //if(ad != null)
            //{
            //    var rest = simpleList.Where(s => s != ad).Append(new Number(NumberFactor)).ToArray();
            //    Add res = (Add) Add.Metadata.CreateInstance();
            //    foreach (var item in ad.Factors.Append(new Number( ad.NumberFactor)))
            //    {
            //        res.AddOperand(new Multiply(rest.Append(item)));
            //    }
            //    res.Seal();
            //    return res;
            //}           
            if (simplified) return new Multiply(simpleList.Append(new Number(NumberFactor)));
            return null;
        }

        void Word.FindX(VariableLocator locator)
        {
            throw new NotImplementedException();
            //locator.Path.Push(this);
            //Utilities.IterateFactors(locator, Factors);
            //if (locator.variable == null) locator.Path.Pop();
        }
        public void Seal()
        {
            IsSealed = true;
            factors?.TrimExcess();
        }

        public Word Convert(Word word, Symbol symbol)
        {
            if (symbol == Symbol.Cross || symbol == Symbol.Null || symbol == Symbol.Point) return word;
            else if (symbol == Symbol.Slash) return new Power(word, new Number(-1));
            else throw new Exception();
        }

        Word IContent.Derivative(Proxy f)
        {

           if(factors.Count == 1)
            {
                return new Multiply((factors[0] as IContent).Derivative(f), new Number(NumberFactor));
            }
            throw new NotImplementedException();
        }

        public Multiply(IEnumerable<Word> words)
        {
            foreach (var item in words)
            {
                AddOperand(item);
            }
        }
        public Multiply(params Word[] words) : this(words as IEnumerable<Word>)
        {
        }       
    }
}
