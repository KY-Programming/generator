using KY.Core;
using KY.Generator.Models;

namespace KY.Generator;

public class LocalDeepSearchAssemblyLocator : LocalAssemblyLocator
{
    public LocalDeepSearchAssemblyLocator(IEnvironment environment)
        : base(environment)
    { }

    public override AssemblyLocation? LocateInDirectory(string path, AssemblyLocateInfo info)
    {
        AssemblyLocation? location = base.LocateInDirectory(path, info);
        if (location != null)
        {
            return location;
        }
        string assemblyFileName = info.Name + ".dll";
        string[] files = Directory.GetFiles(path, assemblyFileName, SearchOption.AllDirectories);
        if (files.Length > 0)
        {
            return new AssemblyLocation(files[0], new SemanticVersion(0), new DotNetVersion(0, 0));
        }
        return null;
    }
}
