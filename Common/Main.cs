using System.Diagnostics;
using System.Reflection;
using KY.Core;
using KY.Core.DataAccess;
using KY.Core.Nuget;

namespace KY.Generator;

public static class Main
{
    private static string CurrentPath { get; } = FileSystem.Combine(Assembly.GetEntryAssembly()?.Location, "..");
    private static string BasePath { get; } = FileSystem.Combine(CurrentPath, "..");
    private static string SharedPath { get; } = FileSystem.Combine(BasePath, "netstandard2.0");

    private static IReadOnlyList<string> IgnoreList { get; } =
    [
        FileSystem.Combine(BasePath, "net461")
    ];

    public static bool Run(string[] args)
    {
#if DEBUG
        if (args.Length > 0 && args[0] != "statistics")
        {
            Debugger.Launch();
        }
#endif
        Generator.InitializeLogger(args);
        NugetPackageDependencyLoader.Activate();
        NugetPackageDependencyLoader.Locations.Insert(0, new SearchLocation(SharedPath));
        Generator generator = Generator.Create();
        bool isX86 = CurrentPath.EndsWith("x86");
        List<string> sharedDirectories = FileSystem.GetDirectories(BasePath)
                                                   .Where(x => !IgnoreList.Contains(x) && x != SharedPath)
                                                   .Where(x => (isX86 && x.EndsWith("x86")) || (!isX86 && !x.EndsWith("x86")))
                                                   .ToList();
        int currentIndex = sharedDirectories.IndexOf(CurrentPath) + 1;
        while (sharedDirectories.Count > currentIndex)
        {
            sharedDirectories.RemoveAt(currentIndex);
        }
        sharedDirectories.Reverse();
        sharedDirectories.Add(SharedPath);
        return generator.SharedAssemblies(sharedDirectories)
                        .PreloadModules("KY.Generator.*.dll")
                        .SetParameters(args)
                        .Run();
    }
}
