using System;
using KY.Core;

namespace KY.Generator
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger.AllTargets.Add(Logger.VisualStudioOutput);

            bool success = Generator.Initialize()
                                    .SetParameters(args)
                                    .Run();
            if (!success)
            {
                Environment.ExitCode = 1;
            }

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
