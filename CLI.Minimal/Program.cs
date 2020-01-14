using System;
using KY.Core;

namespace KY.Generator
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Generator.Create(args)
                     .Run()
                     .SetExitCode();

#if DEBUG
            if (Logger.Console.IsConsoleAvailable)
            {
                Console.WriteLine("Press key to EXIT...");
                Console.ReadKey();
            }
#endif
        }
    }
}