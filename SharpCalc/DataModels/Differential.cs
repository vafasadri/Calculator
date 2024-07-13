using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCalc.DataModels
{
    internal class Differential : Proxy
    {
        public Differential(Proxy parent) : base("d" + parent.Name)
        {
        }
        public override Real Differentiate() => throw new NotImplementedException();
    }
}
