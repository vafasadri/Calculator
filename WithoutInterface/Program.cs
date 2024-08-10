// See https://aka.ms/new-console-template for more information
using SharpCalc;
using SharpCalc.DataModels;

MathParser parser = new();
Console.WriteLine("Note: xyz will not be considered as x * y * z");

IEnvironmentVariable x = parser.RegisterEnvironment("x");
IEnvironmentVariable y = parser.RegisterEnvironment("y");
IEnvironmentVariable t = parser.RegisterEnvironment("t");

while (true)
{
    
    try
    {
        Console.Write(">>> ");

        string expression = Console.ReadLine()!;
        var solve = (Scalar) parser.Run(expression, out bool printInDetails);
        Console.WriteLine(printInDetails ? solve.Print() : solve.Render());
        var xnum = Random.Shared.Next();
        var ynum = Random.Shared.Next();
        var tnum = Random.Shared.Next();
        for (int i = 0; i < 10000000; i++)
        {
            x.SetValue(xnum);
            y.SetValue(ynum);
            t.SetValue(tnum);          
            solve.ComputeNumerically();
            x.Unset();
            y.Unset();
            t.Unset();           
        }
    }
#if !DEBUG
    catch (ApplicationException ex)
    {
        Console.WriteLine(ex.Message);
    }
#endif
    finally
    {

    }
}


