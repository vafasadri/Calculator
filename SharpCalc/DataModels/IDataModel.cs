using System.Runtime.CompilerServices;

namespace SharpCalc.DataModels
{
    public interface IDataModel : IMathNode,INamed
    {
        string IMathNode.ToText() => Name;
    }
}
