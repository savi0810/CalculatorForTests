namespace CalculatorForTests;

internal class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            string ex;
            Console.WriteLine("Введите выражение");
            ex = Console.ReadLine() ?? string.Empty;
            Console.WriteLine(Calculator.Calculate(ex));
        }
    }
}