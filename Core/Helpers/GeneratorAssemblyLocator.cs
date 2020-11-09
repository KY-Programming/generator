using System;
using System.Reflection;
using System.Runtime.Versioning;
using KY.Core;
using KY.Core.Extension;
using KY.Core.Nuget;
using KY.Generator.Models;

namespace KY.Generator
{
    public static class GeneratorAssemblyLocator
    {
        public static Assembly Locate(string assemblyName, GeneratorEnvironment environment, params SearchLocation[] locations)
        {
            NugetAssemblyLocator locator = NugetPackageDependencyLoader.CreateLocator();
            locator.Locations.InsertRange(0, locations);
            Version defaultVersion = typeof(CoreModule).Assembly.GetName().Version;

            Assembly entryAssembly = Assembly.GetEntryAssembly();
            ProcessorArchitecture entryArchitecture = entryAssembly.GetName().ProcessorArchitecture;
            ProcessorArchitecture assemblyArchitecture = AssemblyName.GetAssemblyName(assemblyName).ProcessorArchitecture;
            if (entryArchitecture != assemblyArchitecture)
            {
                environment.SwitchContext = true;
                environment.SwitchToArchitecture = assemblyArchitecture;
                return null;
            }
            Assembly assembly = locator.Locate(assemblyName, defaultVersion);
            FrameworkName assemblyTargetFramework = assembly.GetTargetFramework();
            if (assemblyTargetFramework.IsFramework() && !entryAssembly.GetTargetFramework().IsFramework() && assemblyTargetFramework.Version.Major <= 4)
            {
                environment.SwitchContext = true;
                environment.SwitchToFramework = assemblyTargetFramework;
                return null;
            }
            return assembly;
        }
    }
}