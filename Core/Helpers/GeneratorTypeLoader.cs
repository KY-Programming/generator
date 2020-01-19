using System;
using System.Collections.Generic;
using System.Linq;
using KY.Core;
using KY.Core.Nuget;
using KY.Generator.Configuration;

namespace KY.Generator
{
    public static class GeneratorTypeLoader
    {
        public static Type Get(IConfiguration configuration, string assemblyName, string nameSpace, string typeName, params SearchLocation[] locations)
        {
            List<SearchLocation> list = locations.ToList();
            list.Add(new SearchLocation(configuration.Environment.ConfigurationFilePath));
            list.Add(new SearchLocation(configuration.Environment.OutputPath));
            Version defaultVersion = typeof(CoreModule).Assembly.GetName().Version;
            return NugetPackageTypeLoader.Get(assemblyName, nameSpace, typeName, defaultVersion, list.ToArray());
        }
    }
}