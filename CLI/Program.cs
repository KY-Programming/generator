using KY.Core;
using KY.Core.DataAccess;
using System.Reflection;

namespace KY.Generator;

internal class Program
{
    private static async Task Main(string[] args)
    {
        Generator.InitializeLogger(args);

        string toolDirectory = FileSystem.Parent(Assembly.GetEntryAssembly()!.Location);
        bool success = await Generator.Create()
                                      .PreloadModules(toolDirectory, "KY.Generator.*.dll")
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
