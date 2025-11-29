using KY.Core;

namespace KY.Generator;

public class CachedAssemblyLocator : IAssemblyLocator
{
    public AssemblyLocation? Locate(AssemblyLocateInfo info)
    {
        string? assemblyPath = null; //cache?.Resolve(args.Name);
        return assemblyPath == null ? null : new AssemblyLocation(assemblyPath, new SemanticVersion(0), new DotNetVersion(0, 0));
    }
}
