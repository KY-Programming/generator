using System;
using KY.Core;

namespace KY.Generator.CLI.Core.Minimal
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
            Console.WriteLine("Press key to EXIT...");
            Console.ReadKey();
#endif
        }
    }
}
