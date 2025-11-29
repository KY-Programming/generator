using KY.Core;
using KY.Core.DataAccess;

namespace KY.Generator;

public class RuntimeLocator : IAssemblyLocator
{
    public AssemblyLocation? Locate(AssemblyLocateInfo info)
    {
        return this.LocateRuntime(info) ?? this.LocateRuntime(info, false);
    }

    private AssemblyLocation? LocateRuntime(AssemblyLocateInfo info, bool exactRuntimeVersion = true)
    {
        foreach (InstalledRuntime runtime in InstalledRuntime.GetCurrent())
        {
            DotNetVersion dotNetVersion = DotNetVersion.FromRuntime(runtime);
            if (exactRuntimeVersion && info.Version != null && info.Version.Major != dotNetVersion.Major)
            {
                continue;
            }
            string filePath = FileSystem.Combine(runtime.FullPath, info.Name + ".dll");
            if (FileSystem.FileExists(filePath))
            {
                return new AssemblyLocation(filePath, new SemanticVersion(0), dotNetVersion);
            }
            Logger.Trace($"Assembly not found in path {runtime.FullPath}");
        }
        return null;
    }
}
