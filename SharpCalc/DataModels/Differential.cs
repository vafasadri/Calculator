using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCalc.DataModels
{
    internal class Differential : Variable
    {
        Variable parent;
        public Differential(Variable parent) : base("d" + parent.Name)
        {
            this.parent = parent;
        }
        public override Scalar? Value { get =>
                parent.Value?.Differentiate(); set => throw new Exceptions.CustomError("Cannot assign a value to a differential, integration is not supported"); }
    }
}
