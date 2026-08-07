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

    /// <summary>
    /// Returns the type or throws, if it can not be found. A missing type can not be generated, so the generation has
    /// to fail. Otherwise nothing would be written and the run would still be reported as successful.
    /// </summary>
    /// <exception cref="InvalidOperationException">The type is not part of any loaded assembly</exception>
    public Type Get(string nameSpace, string typeName)
    {
        foreach (Assembly assembly in this.environment.LoadedAssemblies)
        {
            Type? type = assembly.GetType($"{nameSpace}.{typeName}");
            if (type != null)
            {
                return type;
            }
        }
        throw new InvalidOperationException($"Can not find type '{nameSpace}.{typeName}'. Ensure the assembly is loaded via 'load -assembly=<assembly-path>' command before. Loaded assemblies: {string.Join(", ", this.environment.LoadedAssemblies.Select(x => x.GetName().Name))}");
    }
}
