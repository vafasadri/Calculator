﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SharpCalc.Components;
namespace SharpCalc.DataModels
{
    public interface Scalar : IMathNode
    {
        public Scalar Differentiate();
        /// <summary>
        /// when overridden in a deriving class adds all the variables inside the node into the provided set
        /// </summary>
        internal void EnumerateVariables(ISet<Variable> variables);
        internal bool ContainsVariable(Variable variable);
        public bool ContainsVariable(IEnvironmentVariable variable)
        {
            return ContainsVariable((Variable)variable);
        }
        public Complex ComputeNumerically();
    }
}
