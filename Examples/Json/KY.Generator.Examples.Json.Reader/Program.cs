using System;

namespace KY.Generator.Examples.Json.Reader
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Simple with reader");
            SimpleWithReader simpleWithReader = SimpleWithReader.Load("source\\simple.json");
            Console.WriteLine($"string: {simpleWithReader.StringProperty}");
            Console.WriteLine($"number: {simpleWithReader.NumberProperty}");
            Console.WriteLine($"boolean: {simpleWithReader.BooleanProperty}");
            
            Console.WriteLine();
            Console.WriteLine("Simple with extern reader");
            Simple simple = SimpleReader.Load("source\\simple.json");
            Console.WriteLine($"string: {simple.StringProperty}");
            Console.WriteLine($"number: {simple.NumberProperty}");
            Console.WriteLine($"boolean: {simple.BooleanProperty}");

            Console.WriteLine();
            Console.WriteLine("Press key to EXIT...");
            Console.ReadKey();
        }
    }
}