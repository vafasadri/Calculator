using SharpCalc.DataModels;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace SharpCalc.Operators.Arithmetic
{
    internal class Add :  IOperatorGroup,IArithmeticOperator
    {
        public readonly static OperatorGroupMetadata Metadata = OperatorGroupMetadata.Register(
            "Addition", 3,
            () => new Add(),
            true
        );
        internal double NumberFactor;
        private List<Word>? factors;
        internal IReadOnlyList<Word>? Factors => factors;      
        OperatorGroupMetadata IOperatorGroup.Metadata => Metadata;

        public bool IsSealed { get; private set; }

        public string ToText()
        {
            if (factors == null) return NumberFactor.ToString();
            StringBuilder builder = new();
            bool IsFirst = true;
            foreach (var item in factors)
            {
                if (item is Multiply m && m.NumberFactor < 0)
                {
                    if (IsFirst) builder.Append("-");
                    else builder.Append(" - ");
                    var pos = Negative.Create(m).SuperSimplify(out _);
                    builder.Append(this.OpAwarePrint(pos));
                }
                else
                {
                    var f = item.SuperSimplify(out _);
                    if (!IsFirst) builder.Append(" + ");
                    builder.Append(this.OpAwarePrint(f));                  
                }
                IsFirst = false;
            }
            if (NumberFactor != 0)
            {
                builder.Append(NumberFactor < 0 ? " - " : " + ");
                builder.Append(Math.Abs(NumberFactor));
            }
            return builder.ToString();
        }
        public Word? Simplify()
        {
            if (factors == null)
            {
                return new Number(NumberFactor);
            }
            else if (factors.Count == 1 && NumberFactor == 0)
            {
                return factors[0];
            }

            bool simplified = Utilities.SimplifyAny(factors, out Word[] r);
            if (simplified) return new Add(r.Append(new Number(NumberFactor)));
            else return null;
        }

        void Word.FindX(VariableLocator locator)
        {
            throw new NotImplementedException();
            //locator.Path.Push(this);
            //Utilities.IterateFactors(locator, factors?.Select(n => n.));
            //if (locator.variable == null) locator.Path.Pop();
        }
        void InternalAdd(Word item)
        {
            if (factors != null)
                for (int i = 0; i < factors!.Count; i++)
                {
                    bool simplified = true;
                    var eq = Equator.Instance;
                    Word? MergeFactor(Word left, Word right)
                    {
                        if (left is Multiply { Factors: not null } m && m.Factors.Contains(right, eq))
                        {
                            if (m.Factors.Count == 1)
                            {
                                // 2x + x = 3x
                                if (m.IsSealed)
                                {
                                    return new Multiply(new Number(m.NumberFactor + 1), right);
                                }
                                else m.NumberFactor += 1;
                            }
                            else
                            {
                                var others = m.Factors.Where(n => eq.Equals(n, right)).Append(new Number(m.NumberFactor));
                                var final = others.Append(new Add(right, new Number(1)));
                                return new Multiply(final);
                            }
                            return left;
                        }
                        return null;
                    }
                    Word factor = factors[i];
                    Word? temp;                    
                    if (eq.Equals(factor, item))
                    {
                        Multiply m2 = new();
                        m2.AddOperand(new Number(2));
                        m2.AddOperand(factor);
                        factors[i] = m2;
                    }
                    else if ((temp = MergeFactor(item, factor)) != null)
                    {
                        factors[i] = temp;
                    }
                    else if ((temp = MergeFactor(factor, item)) != null)
                    {
                        factors[i] = temp;
                    }
                    else if (item is Multiply { Factors: not null } fi && factor is Multiply { Factors: not null } ff)
                    {
                        var intersect = fi.Factors.Where(n => ff.Factors.Contains(n,eq)).ToList();
                        if (intersect.Any())
                        {
                            
                            var fmul = new Multiply(ff.Factors.Where(n => !intersect.Contains(n,eq)).Append(new Number(ff.NumberFactor))).SuperSimplify(out _);
                            var imul = new Multiply(fi.Factors.Where(n => !intersect.Contains(n,eq)).Append(new Number(fi.NumberFactor))).SuperSimplify(out _);
                            var add = new Add(fmul, imul).SuperSimplify(out _);                          
                            factors[i] = new Multiply(intersect.Append(add));
                        }
                        else continue;
                    }
                    else continue;

                   return;
                }
                
            factors ??= new();
            factors.Add(item);
        }
        public void AddOperand(Word item)
        {
            if (IsSealed) throw new Exceptions.SealedOperatorException();
            switch (item)
            {
                case Add add:
                    if (add.factors != null)
                    {
                        foreach (var factor in add.factors)
                        {
                            AddOperand(factor);
                        }
                    }
                    NumberFactor += add.NumberFactor;
                    break;
                case Number num:
                    NumberFactor += num.Value;
                    break;
                default:
                    InternalAdd(item);
                    break;
            }
        }
        public void Seal()
        {
            IsSealed = true;           
            factors?.TrimExcess();            
        }
        public Word Convert(Word word, Symbol symbol)
        {
            if (symbol == Symbol.Null || symbol == Symbol.Plus)
            {
                return word;
            }
            else if (symbol == Symbol.Minus) return Negative.Create(word);
            else throw new Exception();
        }

        Word IContent.Derivative(Proxy x)
        {
            return new Add(factors?.Select(n => ((IContent)n).Derivative(x)) ?? Enumerable.Empty<Word>());
        }

        public Add(IEnumerable<Word> w)
        {
            foreach (var item in w)
            {
                AddOperand(item);
            }
            Seal();
        }
        public Add(params Word[] words) : this(words as IEnumerable<Word>)
        {

        }
        public Add()
        {
            NumberFactor = 0;
            this.factors = null;
        }
       
    }
}
