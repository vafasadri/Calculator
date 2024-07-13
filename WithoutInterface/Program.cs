// See https://aka.ms/new-console-template for more information
using SharpCalc;
MathParser parser = new();
Console.WriteLine("Note: xyz will not be considered as x * y * z");
while (true)
{

    //try
    //{
    Console.Write(">>> ");

    string expression = Console.ReadLine()!;
    
    var solve = parser.Run(expression, out bool printInDetails);
    Console.WriteLine(printInDetails ? solve.Print() : solve.ToText());
        
    //}
//    catch (
//#if DEBUG
//ApplicationException
//#else 
//Exception
//#endif
//    ex)
//    {
//        Console.WriteLine(ex.Message);
//    }

}


