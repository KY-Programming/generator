using System.Diagnostics;
using System.Reflection;
using KY.Core.DataAccess;

namespace KY.Generator;

public static class Main
{
    private static string SharedPath { get; } = FileSystem.Combine(Assembly.GetEntryAssembly().Location, "..\\..\\netstandard2.0");

    public static bool Run(string[] args)
    {
#if DEBUG
        if (args.Length > 0 && args[0] != "statistics")
        {
            Debugger.Launch();
        }
#endif
        Generator.InitializeLogger(args);
        return Generator.Create()
                        .SharedAssemblies(SharedPath)
                        .PreloadModules(SharedPath, "KY.Generator.*.dll")
                        .SetParameters(args)
                        .Run();
    }
}
