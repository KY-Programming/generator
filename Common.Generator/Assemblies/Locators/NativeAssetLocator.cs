using System.Runtime.InteropServices;
using KY.Core;
using KY.Core.DataAccess;
using Newtonsoft.Json;

namespace KY.Generator;

/// <summary>
/// Locates the runtime specific assets of the users project: the native binary (e.g. e_sqlite3) a managed assembly
/// has to P/Invoke into, and the runtime specific build of a managed assembly itself (e.g. Microsoft.Data.SqlClient,
/// whose assembly next to the deps.json is a facade that throws on every platform).
/// </summary>
/// <remarks>
/// Runtime specific assets are resolved by the .NET host from the deps.json of the <b>running application</b>. The
/// generator loads assemblies of the users project into its own process, so its host never learns about their assets
/// and every P/Invoke ends in a DllNotFoundException - or, for a managed asset, the platform neutral facade is loaded
/// and throws a PlatformNotSupportedException. This locator reads the deps.json of the users project instead and
/// returns the path the host would have chosen.
/// </remarks>
public class NativeAssetLocator
{
    private const string NativeAssetType = "native";
    private const string RuntimeAssetType = "runtime";

    /// <summary>
    /// Runtime identifiers to search for, most specific first (e.g. win-x64, win).
    /// </summary>
    public static List<string> RuntimeIdentifiers { get; } = BuildRuntimeIdentifiers();

    /// <summary>
    /// Returns the absolute path of the native library or null if no matching asset was found.
    /// </summary>
    /// <param name="applicationDirectory">Output directory of the users project - the one containing its deps.json.</param>
    /// <param name="libraryName">Name as used in the DllImport, without prefix and extension (e.g. e_sqlite3).</param>
    public string? Locate(string applicationDirectory, string libraryName)
    {
        List<string> fileNames = BuildFileNames(libraryName);
        return LocateByDepsFile(applicationDirectory, fileNames, NativeAssetType) ?? LocateByConvention(applicationDirectory, fileNames);
    }

    /// <summary>
    /// Returns the absolute path of the runtime specific build of a managed assembly, or null when the package has
    /// none - then the assembly next to the deps.json is the real one and nothing has to be redirected.
    /// </summary>
    /// <param name="applicationDirectory">Output directory of the users project - the one containing its deps.json.</param>
    /// <param name="assemblyName">Assembly name without extension (e.g. Microsoft.Data.SqlClient).</param>
    public string? LocateRuntimeAssembly(string applicationDirectory, string assemblyName)
    {
        // Only the deps.json can tell a runtime specific build apart from the facade of the same name - a file
        // search would find whichever comes first, which is what has to be avoided here.
        return LocateByDepsFile(applicationDirectory, [$"{assemblyName}.dll"], RuntimeAssetType);
    }

    private static string? LocateByDepsFile(string applicationDirectory, List<string> fileNames, string assetType)
    {
        foreach (string depsFile in FileSystem.GetFiles(applicationDirectory, "*.deps.json"))
        {
            DependencyContext? context = JsonConvert.DeserializeObject<DependencyContext>(FileSystem.ReadAllText(depsFile));
            if (context?.Targets == null)
            {
                continue;
            }
            // Every runtime identifier is listed in the deps.json, the most specific match wins
            foreach (string runtimeIdentifier in RuntimeIdentifiers)
            {
                foreach (KeyValuePair<string, DependencyTarget> package in context.Targets.Values.SelectMany(target => target))
                {
                    if (package.Value.RuntimeTargets == null)
                    {
                        continue;
                    }
                    foreach (KeyValuePair<string, DependencyRuntimeTarget> asset in package.Value.RuntimeTargets)
                    {
                        if (!assetType.Equals(asset.Value.AssetType, StringComparison.OrdinalIgnoreCase)
                            || !runtimeIdentifier.Equals(asset.Value.RuntimeIdentifier, StringComparison.OrdinalIgnoreCase)
                            || !fileNames.Contains(FileSystem.GetFileName(asset.Key)))
                        {
                            continue;
                        }
                        string? path = Resolve(applicationDirectory, asset.Key, context, package.Key);
                        if (path != null)
                        {
                            return path;
                        }
                        Logger.Trace($"Native asset {asset.Key} of {package.Key} is declared in {FileSystem.GetFileName(depsFile)}, but was not found on disk");
                    }
                }
            }
        }
        return null;
    }

    /// <summary>
    /// A framework dependent build keeps the runtimes/{rid}/native/ structure, a published one flattens it into the
    /// application directory. If neither is present, the asset is read from the NuGet cache - the package it belongs
    /// to is known from the deps.json, so nothing has to be guessed.
    /// </summary>
    private static string? Resolve(string applicationDirectory, string relativePath, DependencyContext context, string packageKey)
    {
        string[] chunks = relativePath.Split('/');
        string path = Combine(applicationDirectory, chunks);
        if (FileSystem.FileExists(path))
        {
            return path;
        }
        path = FileSystem.Combine(applicationDirectory, chunks.Last());
        if (FileSystem.FileExists(path))
        {
            return path;
        }
        string? packagePath = context.Libraries != null && context.Libraries.TryGetValue(packageKey, out DependencyLibrary? library) ? library.Path : null;
        if (packagePath == null)
        {
            return null;
        }
        foreach (string cachePath in GetNugetCachePaths())
        {
            path = Combine(cachePath, packagePath.Split('/').Concat(chunks).ToArray());
            if (FileSystem.FileExists(path))
            {
                return path;
            }
        }
        return null;
    }

    private static string? LocateByConvention(string applicationDirectory, List<string> fileNames)
    {
        foreach (string runtimeIdentifier in RuntimeIdentifiers)
        {
            foreach (string fileName in fileNames)
            {
                string path = FileSystem.Combine(applicationDirectory, "runtimes", runtimeIdentifier, "native", fileName);
                if (FileSystem.FileExists(path))
                {
                    return path;
                }
            }
        }
        foreach (string fileName in fileNames)
        {
            string path = FileSystem.Combine(applicationDirectory, fileName);
            if (FileSystem.FileExists(path))
            {
                return path;
            }
        }
        return null;
    }

    private static IEnumerable<string> GetNugetCachePaths()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            yield return NugetAssemblyLocator.WindowsNugetCachePath;
            yield return NugetAssemblyLocator.WindowsNugetFallbackPath;
        }
        else
        {
            yield return NugetAssemblyLocator.LinuxNugetCachePath;
        }
    }

    private static string Combine(string path, IEnumerable<string> chunks)
    {
        return FileSystem.Combine(new[] { path }.Concat(chunks).ToArray());
    }

    private static List<string> BuildFileNames(string libraryName)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return [$"{libraryName}.dll", libraryName];
        }
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return [$"lib{libraryName}.dylib", $"{libraryName}.dylib", libraryName];
        }
        return [$"lib{libraryName}.so", $"{libraryName}.so", libraryName];
    }

    private static List<string> BuildRuntimeIdentifiers()
    {
        string operatingSystem = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "win"
            : RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? "osx"
            : "linux";
        // The process architecture is used, not the one of the operating system, because the generator also ships as
        // x86 executable and would otherwise pick the x64 binaries on a x64 machine
        string architecture = RuntimeInformation.ProcessArchitecture switch
        {
            Architecture.X86 => "x86",
            Architecture.X64 => "x64",
            Architecture.Arm => "arm",
            Architecture.Arm64 => "arm64",
            _ => RuntimeInformation.ProcessArchitecture.ToString().ToLowerInvariant()
        };
        List<string> identifiers = [$"{operatingSystem}-{architecture}", operatingSystem];
        if (operatingSystem != "win")
        {
            // Some packages group everything non Windows under "unix" instead of naming the platform - most
            // specific still wins, so this only applies when nothing above matched.
            identifiers.Add("unix");
        }
        return identifiers;
    }
}

public class DependencyContext
{
    [JsonProperty("targets")]
    public Dictionary<string, Dictionary<string, DependencyTarget>>? Targets { get; set; }

    [JsonProperty("libraries")]
    public Dictionary<string, DependencyLibrary>? Libraries { get; set; }
}

public class DependencyTarget
{
    [JsonProperty("runtimeTargets")]
    public Dictionary<string, DependencyRuntimeTarget>? RuntimeTargets { get; set; }
}

public class DependencyRuntimeTarget
{
    [JsonProperty("rid")]
    public string? RuntimeIdentifier { get; set; }

    [JsonProperty("assetType")]
    public string? AssetType { get; set; }
}

public class DependencyLibrary
{
    [JsonProperty("path")]
    public string? Path { get; set; }
}
