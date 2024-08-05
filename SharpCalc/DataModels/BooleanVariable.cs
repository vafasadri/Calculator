using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCalc.DataModels;

internal class BooleanVariable : Proxy,Statement
{
    public new Statement? Value { get => (Statement?)base.Value; set => base.Value = value; }
    public override string TypeName => "Statement";
    public BooleanVariable(string name) : base(name)
    {

    }
}  
