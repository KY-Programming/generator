using System.Runtime.InteropServices;
using KY.Core;
using KY.Core.DataAccess;

namespace KY.Generator;

public class NugetAssemblyLocator : IAssemblyLocator
{
    public static readonly string WindowsNugetCachePath = FileSystem.Combine(Environment.ExpandEnvironmentVariables("%USERPROFILE%"), ".nuget", "packages");
    public static readonly string WindowsNugetFallbackPath = FileSystem.Combine(Environment.ExpandEnvironmentVariables("%PROGRAMFILES%"), "dotnet", "sdk", "NuGetFallbackFolder");
    public static readonly string LinuxNugetCachePath = FileSystem.Combine(Environment.GetEnvironmentVariable("HOME") ?? string.Empty, ".nuget", "packages");

    public static List<string> InPackageFolders { get; } =
    [
        "lib", "ref", "generators", "tools"
    ];

    public static List<string> IgnoredPackageNamePostfixes { get; } =
    [
        ".Fluent",
        ".Generator"
    ];

    public AssemblyLocation? Locate(AssemblyLocateInfo info)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return this.LocateWindows(info);
        }
        return this.LocateLinux(info);
    }

    private AssemblyLocation? LocateWindows(AssemblyLocateInfo info)
    {
        return this.Locate(WindowsNugetCachePath, info) ?? this.Locate(WindowsNugetFallbackPath, info);
    }

    private AssemblyLocation? LocateLinux(AssemblyLocateInfo info)
    {
        return this.Locate(LinuxNugetCachePath, info);
    }

    private AssemblyLocation? Locate(string path, AssemblyLocateInfo info)
    {
        string packageName = info.Name.ToLowerInvariant();
        string packagePath = FileSystem.Combine(path, packageName);
        if (FileSystem.DirectoryExists(packagePath))
        {
            AssemblyLocation? location = this.LocatePackageVersion(packagePath, info);
            if (location != null)
            {
                return location;
            }
        }
        foreach (string postfix in IgnoredPackageNamePostfixes)
        {
            packagePath = FileSystem.Combine(path, packageName.TrimEnd(postfix.ToLowerInvariant()));
            if (!FileSystem.DirectoryExists(packagePath))
            {
                continue;
            }
            AssemblyLocation? location = this.LocatePackageVersion(packagePath, info);
            if (location != null)
            {
                return location;
            }
        }
        Logger.Trace($"Assembly not found in path {FileSystem.Combine(path, packageName, info.Version?.ToString() ?? "<anyVersion>", info.DotNetVersion?.ToString() ?? "<anyDotNetVersion>")}");
        return null;
    }

    private AssemblyLocation? LocatePackageVersion(string packagePath, AssemblyLocateInfo info)
    {
        List<SemanticVersion> versions = FileSystem.GetDirectoryInfos(packagePath).Select(x => new SemanticVersion(x.Name)).ToList();

        SemanticVersion? closest = info.Version == null ? versions.Newest() : versions.Closest(info.Version);

        return closest == null ? null : this.LocateDotNetVersion(FileSystem.Combine(packagePath, closest.ToString()), info, closest);
    }

    private AssemblyLocation? LocateDotNetVersion(string packagePath, AssemblyLocateInfo info, SemanticVersion packageVersion)
    {
        foreach (string folder in InPackageFolders)
        {
            string folderPath = FileSystem.Combine(packagePath, folder);
            if (!FileSystem.DirectoryExists(folderPath))
            {
                continue;
            }
            List<DotNetVersion> versions = FileSystem.GetDirectoryInfos(folderPath).Select(DotNetVersion.FromDirectory).OfType<DotNetVersion>().ToList();
            DotNetVersion? version = info.DotNetVersion == null ? versions.Newest() : versions.Closest(info.DotNetVersion) ?? versions.ClosestNewer(info.DotNetVersion);
            if (version == null)
            {
                continue;
            }
            AssemblyLocation? location = this.LocateAssembly(FileSystem.Combine(folderPath, version.ToString()), info, packageVersion, version);
            if (location != null)
            {
                return location;
            }
        }
        return null;
    }

    private AssemblyLocation? LocateAssembly(string packagePath, AssemblyLocateInfo info, SemanticVersion packageVersion, DotNetVersion dotNetVersion)
    {
        string filePath = FileSystem.Combine(packagePath, $"{info.Name}.dll");
        return FileSystem.FileExists(filePath) ? new AssemblyLocation(filePath, packageVersion, dotNetVersion) : null;
    }
}
