using System.Reflection;

namespace KY.Generator;

public class AssemblyLoadInfo
{
    public Assembly? Assembly { get; set; }
    public string? AssemblyPath { get; set; }
    public bool Cancel { get; set; }

    public static AssemblyLoadInfo Success(Assembly assembly)
    {
        return new AssemblyLoadInfo
        {
            Assembly = assembly
        };
    }

    public static AssemblyLoadInfo Success(string assemblyPath)
    {
        return new AssemblyLoadInfo
        {
            AssemblyPath = assemblyPath
        };
    }

    public static AssemblyLoadInfo Cancelled()
    {
        return new AssemblyLoadInfo
        {
            Cancel = true
        };
    }
}
