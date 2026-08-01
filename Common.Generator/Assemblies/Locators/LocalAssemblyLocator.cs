using System.Reflection;
using KY.Core;
using KY.Core.DataAccess;
using KY.Generator.Models;

namespace KY.Generator;

public class LocalAssemblyLocator : IAssemblyLocator
{
    private readonly IEnvironment environment;

    public LocalAssemblyLocator(IEnvironment environment)
    {
        this.environment = environment;
    }

    public AssemblyLocation? Locate(AssemblyLocateInfo info)
    {
        if (info.Hint != null && FileSystem.FileExists(info.Hint))
        {
            return new AssemblyLocation(info.Hint, new SemanticVersion(0), new DotNetVersion(0, 0));
        }
        List<string?> paths =
        [
            Environment.CurrentDirectory,
            FileSystem.GetDirectoryName(Assembly.GetCallingAssembly().Location),
            FileSystem.GetDirectoryName(Assembly.GetEntryAssembly()?.Location)
        ];
        if (info.Hint != null && FileSystem.DirectoryExists(info.Hint))
        {
            paths.Add(info.Hint);
        }
        foreach (Assembly assembly in this.environment.LoadedAssemblies)
        {
            paths.Add(assembly.Location);
        }
        IEnumerable<string> uniquePaths = paths.OfType<string>().Select(FileSystem.FormatPath).Unique();
        foreach (string path in uniquePaths)
        {
            AssemblyLocation? assemblyLocation = this.LocateInDirectory(path, info);
            if (assemblyLocation != null)
            {
                return assemblyLocation;
            }
            Logger.Trace($"Assembly not found in path {path}");
        }
        return null;
    }

    public virtual AssemblyLocation? LocateInDirectory(string path, AssemblyLocateInfo info)
    {
        return LocateInFrameworkDirectories(path, info) ?? LocateFile(path, new DotNetVersion(0, 0), info);
    }

    /// <summary>
    /// Local build output is split per target framework (bin/{configuration}/{framework}/), so the same assembly
    /// exists multiple times with different capabilities. Which folder is probed first depends on the load order, so
    /// the framework closest to the running runtime is picked here - the same rule the
    /// <see cref="NugetAssemblyLocator" /> applies to the lib/{framework} folders of a package.
    /// </summary>
    private static AssemblyLocation? LocateInFrameworkDirectories(string path, AssemblyLocateInfo info)
    {
        if (DotNetVersion.FromDirectoryName(FileSystem.GetFileName(path)) == null)
        {
            return null;
        }
        string parentPath = FileSystem.Parent(path);
        List<DotNetVersion> versions = [];
        Dictionary<DotNetVersion, string> paths = new();
        foreach (DirectoryInfo directory in FileSystem.GetDirectoryInfos(parentPath))
        {
            DotNetVersion? version = DotNetVersion.FromDirectory(directory);
            if (version == null || !FileSystem.FileExists(FileSystem.Combine(directory.FullName, info.Name + ".dll")))
            {
                continue;
            }
            versions.Add(version);
            paths[version] = directory.FullName;
        }
        DotNetVersion? closest = info.DotNetVersion == null ? versions.Newest() : versions.Closest(info.DotNetVersion) ?? versions.ClosestNewer(info.DotNetVersion);
        return closest == null ? null : LocateFile(paths[closest], closest, info);
    }

    private static AssemblyLocation? LocateFile(string path, DotNetVersion dotNetVersion, AssemblyLocateInfo info)
    {
        string assemblyFileName = info.Name + ".dll";
        string filePath = FileSystem.Combine(path, assemblyFileName);
        if (FileSystem.FileExists(filePath))
        {
            return new AssemblyLocation(filePath, new SemanticVersion(0), dotNetVersion);
        }
        return null;
    }
}
