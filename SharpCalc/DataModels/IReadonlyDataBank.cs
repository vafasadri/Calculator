using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCalc.DataModels
{
    public interface IReadonlyDataBank : IEnumerable<IDataModel>
    {
        bool ContainsName(string name);
        IDataModel? GetData(string name);
    }
}
