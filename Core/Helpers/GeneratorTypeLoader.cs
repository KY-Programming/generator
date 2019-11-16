using System;
using System.Collections.Generic;
using System.Linq;
using KY.Core;
using KY.Core.Nuget;
using KY.Generator.Configuration;
using KY.Generator.Configurations;

namespace KY.Generator
{
    public static class GeneratorTypeLoader
    {
        public static Type Get(ConfigurationBase configuration, string assemblyName, string nameSpace, string typeName, params SearchLocation[] locations)
        {
            List<SearchLocation> list = locations.ToList();
            list.Add(new SearchLocation(configuration.Environment.ConfigurationFilePath));
            list.Add(new SearchLocation(configuration.Environment.OutputPath));
            Version defaultVersion = typeof(CoreModule).Assembly.GetName().Version;
            return NugetPackageTypeLoader.Get(assemblyName, nameSpace, typeName, defaultVersion, list.ToArray());
        }
    }
}
