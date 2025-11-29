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
        string assemblyFileName = info.Name + ".dll";
        string filePath = FileSystem.Combine(path, assemblyFileName);
        if (FileSystem.FileExists(filePath))
        {
            return new AssemblyLocation(filePath, new SemanticVersion(0), new DotNetVersion(0, 0));
        }
        return null;
    }
}
