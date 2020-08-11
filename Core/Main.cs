using System.Reflection;
using KY.Core;
using KY.Core.DataAccess;
using KY.Core.Nuget;

namespace KY.Generator
{
    public static class Main
    {
        private static string SharedPath { get; } = FileSystem.Combine(Assembly.GetEntryAssembly().Location, "..\\..\\netstandard2.0");

        public static bool Run(string[] args)
        {
            Generator.InitializeLogger(args);
            NugetPackageDependencyLoader.Activate();
            NugetPackageDependencyLoader.Locations.Insert(0, new SearchLocation(SharedPath));
            return Generator.Initialize()
                            .PreloadModules(SharedPath, "KY.Generator.*.dll")
                            .SetParameters(args)
                            .Run();
        }
    }
}