using KY.Core;
using KY.Core.Nuget;

namespace KY.Generator;

public static class GeneratorTypeLoader
{
    public static Type? Get(string assemblyName, string nameSpace, string typeName, params SearchLocation[] locations)
    {
        List<SearchLocation> list = locations.ToList();
        // TODO: Check if alternative is required
        //list.Add(new SearchLocation(configuration.GeneratorEnvironment.ConfigurationFilePath));
        //list.Add(new SearchLocation(configuration.GeneratorEnvironment.OutputPath));
        Version defaultVersion = typeof(CoreModule).Assembly.GetName().Version;
        return NugetPackageTypeLoader.Get(assemblyName, nameSpace, typeName, defaultVersion, list.ToArray());
    }
}
