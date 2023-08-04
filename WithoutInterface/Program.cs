// See https://aka.ms/new-console-template for more information
using SharpCalc;
MathParser parser = new();
Console.WriteLine("Note: xyz will not be considered as x * y * z");
while (true)
{
    
    Console.Write(">>> ");

    string expression = Console.ReadLine()!;
    var solve = parser.Run(expression, out bool isSimplified);
    Console.WriteLine(isSimplified ? solve.ToText() : solve.Print());
}


