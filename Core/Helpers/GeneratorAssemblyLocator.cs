using System;
using System.Reflection;
using KY.Core;

namespace KY.Generator
{
    public static class GeneratorAssemblyLocator
    {
        public static Assembly Locate(string assemblyName, params string[] locations)
        {
            NugetAssemblyLocator locator = NugetPackageDependencyLoader.CreateLocator();
            locator.Locations.InsertRange(0, locations);
            Version defaultVersion = typeof(CoreModule).Assembly.GetName().Version;
            return locator.Locate(assemblyName, defaultVersion);
        }
    }
}