using System;
using System.Reflection;
using System.Runtime.Versioning;
using KY.Core;
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

            ProcessorArchitecture entryArchitecture = Assembly.GetEntryAssembly().GetName().ProcessorArchitecture;
            ProcessorArchitecture assemblyArchitecture = AssemblyName.GetAssemblyName(assemblyName).ProcessorArchitecture;
            if (entryArchitecture != assemblyArchitecture)
            {
                environment.SwitchContext = true;
                environment.SwitchToArchitecture = assemblyArchitecture;
                return null;
            }
            Assembly assembly = locator.Locate(assemblyName, defaultVersion);
            // TODO: Implement switch for different frameworks
            // TargetFrameworkAttribute targetFrameworkAttribute = assembly.GetCustomAttribute<TargetFrameworkAttribute>();
            // if(targetFrameworkAttribute.)
            return assembly;
        }
    }
}