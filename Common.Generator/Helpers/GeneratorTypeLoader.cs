using System.Reflection;
using KY.Core;
using KY.Generator.Helpers;
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
    /// <para>
    /// The type name is the one used in the source code, so a nested type is written as 'Outer.Inner'. The CLR
    /// spells it 'Outer+Inner', which <see cref="Assembly.GetType(string)"/> insists on - the nested types are
    /// searched separately for that reason.
    /// </para>
    /// </summary>
    /// <exception cref="InvalidOperationException">The type is not part of any loaded assembly</exception>
    public Type Get(string nameSpace, string typeName)
    {
        foreach (Assembly assembly in this.environment.LoadedAssemblies)
        {
            Type? type = assembly.GetType($"{nameSpace}.{typeName}") ?? FindNestedType(assembly, nameSpace, typeName);
            if (type != null)
            {
                return type;
            }
        }
        throw new InvalidOperationException($"Can not find type '{nameSpace}.{typeName}'. Ensure the assembly is loaded via 'load -assembly=<assembly-path>' command before. Loaded assemblies: {string.Join(", ", this.environment.LoadedAssemblies.Select(x => x.GetName().Name))}");
    }

    private static Type? FindNestedType(Assembly assembly, string nameSpace, string typeName)
    {
        if (!typeName.Contains("."))
        {
            return null;
        }
        return TypeHelper.GetTypes(assembly)
                         .FirstOrDefault(x => x.IsNested && x.Namespace == nameSpace && TypeHelper.GetSourceName(x) == typeName);
    }
}
