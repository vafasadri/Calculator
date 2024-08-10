using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SharpCalc.DataModels
{
    public interface IEnvironmentVariable
    {
        void SetValue(double value);
        void Unset();
        public void SetDifferential(Scalar value);
        public void UnsetDifferential();
        public void Allow();
        public void Disallow();
    }
    internal class EnivronmentVariable : Variable,IEnvironmentVariable
    {
        public override Scalar? Value { get => null; set => throw new Exceptions.CustomError("cannot set the value of an environment variable"); }
        Components.Complex m_value;
        bool m_set = false;
        bool m_diff = false;
        bool m_allowed = false;
        Scalar? m_diffValue;
        public void SetValue(double value)
        {
            m_value.b = value;
            m_set = true;
        }
        public void Unset()
        {
            m_set = false;
        }
        public void SetDifferential(Scalar value)
        {
            m_diff = true;
            m_diffValue = value;
        }
        public void UnsetDifferential()
        {
            m_diff = false;
        }
        public override Components.Complex ComputeNumerically()
        {
            if (m_set) return m_value;
            else throw new Exception();
        }
        public override Scalar Differentiate()
        {
            if (m_diff) return m_diffValue!;
            else return base.Differentiate();
        }
        public override IMathNode? SimplifyInternal()
        {
            if (m_allowed) return null;
            else throw new Exception();
        }
        public void Allow() => m_allowed = true;
        public void Disallow() => m_allowed = false;

        public EnivronmentVariable(string name) : base(name)
        {

        }
    }
}
