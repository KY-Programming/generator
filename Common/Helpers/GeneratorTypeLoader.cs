using System.Reflection;
using KY.Core;
using KY.Generator.Models;

namespace KY.Generator;

public class GeneratorTypeLoader
{
    private readonly IEnvironment environment;

    public GeneratorTypeLoader(IEnvironment environment)
    {
        this.environment = environment;
    }

    public Type? Get(string nameSpace, string typeName)
    {
        foreach (Assembly assembly in this.environment.LoadedAssemblies)
        {
            Type? type = assembly.GetType($"{nameSpace}.{typeName}");
            if (type != null)
            {
                return type;
            }
        }
        Logger.Error($"Can not find type '{nameSpace}.{typeName}'. Ensure the assembly is loaded via 'load -assembly=<assembly-path>' command before.");
        return null;
    }
}
