using SharpCalc.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCalc.DataModels
{
    public abstract class Proxy : IDataModel
    {
        public IMathNode? Value { get; set; }
        public string Name { get; }
        public virtual string TypeName => "Proxy";
        public virtual bool Equals(IMathNode? other) => ReferenceEquals(this, other) || Equator.Equals(Value, other);
        public string Render()
        {
            return Name;
        }
        public string Print()
        {
            return $"{Name} =>  {Value?.Render() ?? "?"}";
        }
        public virtual IMathNode? SimplifyInternal()
        {
            return Value;
        }
        public Proxy(string name)
        {
            Name = name;
        }
    }
}
