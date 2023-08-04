using SharpCalc.DataModels;
using System.Collections;
using System.Reflection.Metadata.Ecma335;

namespace SharpCalc;

readonly struct ShallowName : INamed
{
    public string Name { get; }

    public ShallowName(string name)
    {
        Name = name;
    }
}
/// <summary>
/// keeps track of variables and functions, creates a new variable when a new variable like x is mentioned inside math expressions
/// and retrieves the variable in later uses inside expressions
/// </summary>
internal class DataBank : IReadonlyDataBank
{
    private class MyEquator : IComparer<INamed>
    {
        public int Compare(INamed? x, INamed? y)
        {
            return string.Compare(x?.Name, y?.Name);
        }
    }
    private static readonly MyEquator equator = new();
    protected readonly SortedSet<INamed> items;
    public IEnumerable<Variable> Variables => items.OfType<Variable>();
    public int Count => items.Count;

    public bool AddFunction(IFunction data)
    {
        if (items.Add(data))
        {
            return true;
        }
        else if (data is IFunction && GetData(new ShallowName(data.Name)) is not IFunction _ and not Variable { Value: not null })
        {
            items.Remove(data);
            items.Add(data);
            return true;
        }
        return false;
    }


    public bool ContainsName(ShallowName name)
    {
        return items.Contains(name);
    }
    public IDataModel? GetData(ShallowName name)
    {
        items.TryGetValue(name, out INamed? result);
        return result as IDataModel;
    }
    public IDataModel ForceGetData(ShallowName name)
    {
        IDataModel? result = GetData(name);
        result ??= CreateAbstract(name);
        return result;
    }
    protected Variable CreateAbstract(ShallowName name)
    {
        Variable newab = new(name.Name);
        items.Add(newab);
        return newab;
    }
    public bool AddData(IDataModel data)
    {

        return items.Add(data);
                  
    }

    bool IReadonlyDataBank.ContainsName(string name)
    {
        return ContainsName(new ShallowName(name));
    }
    IDataModel? IReadonlyDataBank.GetData(string name)
    {
        return GetData(new ShallowName(name));
    }

    IEnumerator<IDataModel> IEnumerable<IDataModel>.GetEnumerator()
    {
        return items.Cast<IDataModel>().GetEnumerator();
    }

    public IEnumerator GetEnumerator()
    {
        return items.GetEnumerator();
    }

    // clone constructor
    public DataBank()
    {
        items = new(StaticData.DataBank, equator);
    }
}


