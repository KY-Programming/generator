using System.Diagnostics;
using System.Reflection;
using KY.Core.DataAccess;

namespace KY.Generator;

public static class Main
{
    private static string CurrentPath { get; } = FileSystem.Combine(Assembly.GetEntryAssembly()?.Location, "..");
    private static string BasePath { get; } = FileSystem.Combine(CurrentPath, "..");
    private static string SharedPath { get; } = FileSystem.Combine(BasePath, "netstandard2.0");

    private static IReadOnlyList<string> IgnoreList { get; } =
    [
        FileSystem.Combine(BasePath, "net462")
    ];

    public static async Task<bool> Run(string[] args)
    {
#if DEBUG
        if (args.Length > 0 && args[0] != "statistics")
        {
            Debugger.Launch();
        }
#endif
        Generator.InitializeLogger(args);
        Generator generator = Generator.Create();
        bool isX86 = CurrentPath.EndsWith("x86");
        List<string> sharedDirectories = FileSystem.GetDirectories(BasePath)
                                                   .Where(x => !IgnoreList.Contains(x) && x != SharedPath)
                                                   .Where(x => isX86 && x.EndsWith("x86") || !isX86 && !x.EndsWith("x86"))
                                                   .ToList();
        int currentIndex = sharedDirectories.IndexOf(CurrentPath) + 1;
        while (sharedDirectories.Count > currentIndex)
        {
            sharedDirectories.RemoveAt(currentIndex);
        }
        sharedDirectories.Reverse();
        sharedDirectories.Add(SharedPath);
        return await generator.SharedAssemblies(sharedDirectories)
                              .SetParameters(args)
                              .Run();
    }
}
