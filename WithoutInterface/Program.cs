// See https://aka.ms/new-console-template for more information
using SharpCalc;
using SharpCalc.DataModels;

MathParser parser = new();
Console.WriteLine("Note: xyz will not be considered as x * y * z");
while (true)
{   
    try
    {
        Console.Write(">>> ");

        string expression = Console.ReadLine()!;
        var solve = (Scalar) parser.Run(expression, out bool printInDetails);
        Console.WriteLine(printInDetails ? solve.Print() : solve.Render());
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


