using System;
using KY.Core;
using KY.Generator.Watchdog;

namespace KY.Generator
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Logger.AllTargets.Add(Logger.VisualStudioOutput);

            bool success = Generator.Initialize()
                                    .PreloadModule<WatchdogModule>()
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