using SharpCalc.DataModels;
using SharpCalc.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpCalc.Components;
namespace SharpCalc.Functions;

internal class Min : IFunction
{
    public static readonly IFunction Instance = new Min();
    public int ParameterCount => -1;
    public string Name => "min";
    public Scalar Differentiate(ReadOnlySpan<IMathNode> parameter) => null;
    public bool Equals(IMathNode? other) => throw new NotImplementedException();
    public IFunction GetDerivative() => throw new NotImplementedException();
    public Scalar? Reverse(Scalar factor, Scalar target, ReadOnlySpan<IMathNode> paramlist) => null;
    public Scalar? Run(ReadOnlySpan<IMathNode> paramlist)
    {
        Complex min;
        try
        {
            min = ((Scalar)paramlist[0]).ComputeNumerically();
        }
        catch
        {
            return null;
        }
        if (!min.IsReal()) throw new CustomError("Cannot compare two complex numbers");
        foreach (var e in paramlist)
        {
            Complex item;
            try
            {
                item = ((Scalar)e).ComputeNumerically();
            }
            catch
            {
                return null;
            }
            if (!item.IsReal()) throw new CustomError("Cannot compare two complex numbers");
            if (item.b < min.b) min = item;
        }
        return new Number(min);
    }
    public Complex RunFast(ReadOnlySpan<Complex> paramlist)
    {
        var min = paramlist[0];
        if (!min.IsReal()) throw new CustomError("Cannot compare two complex numbers");
        foreach (var item in paramlist)
        {
            if (!item.IsReal()) throw new CustomError("Cannot compare two complex numbers");
            if (item.b < min.b) min = item;
        }
        return min;
    }
    public string Print()
    {
        return "min(a,b,c,...) : Least number among a,b,c,...";
    }
    private Min()
    {

    }
}
