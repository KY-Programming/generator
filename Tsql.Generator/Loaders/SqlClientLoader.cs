using System.Reflection;
using KY.Core;
using KY.Core.DataAccess;
using KY.Generator.Models;

namespace KY.Generator.Tsql.Loaders;

/// <summary>
/// Loads Microsoft.Data.SqlClient out of the users project - the runtime specific build of it, not the facade.
/// </summary>
/// <remarks>
/// The assembly that sits next to the users deps.json is platform neutral and throws a
/// <see cref="PlatformNotSupportedException"/> from every constructor; the working build lives in
/// runtimes/{rid}/lib/{framework}/ and is picked by the .NET host from the deps.json. The generator loads the users
/// assemblies into its own process, so its host never does that and the facade would win by file name - see
/// <see cref="NativeAssetLocator"/>.
/// <para>
/// Has to run before the first type that references SqlClient is JIT compiled, which is why this class touches no
/// SqlClient type itself and is called before the reader is created.
/// </para>
/// </remarks>
public class SqlClientLoader
{
    private const string AssemblyName = "Microsoft.Data.SqlClient";

    /// <summary>Name as used in the DllImport of SqlClient, without the extension.</summary>
    private const string NativeLibraryName = "Microsoft.Data.SqlClient.SNI";

    private static bool loaded;

    private readonly NativeAssetLocator locator;
    private readonly NativeLibraryLoader nativeLibraryLoader;
    private readonly IEnvironment environment;

    public SqlClientLoader(NativeAssetLocator locator, NativeLibraryLoader nativeLibraryLoader, IEnvironment environment)
    {
        this.locator = locator;
        this.nativeLibraryLoader = nativeLibraryLoader;
        this.environment = environment;
    }

    public void Load()
    {
        if (loaded)
        {
            return;
        }
        loaded = true;
        foreach (string directory in this.GetApplicationDirectories())
        {
            string? path = this.locator.LocateRuntimeAssembly(directory, AssemblyName);
            if (path == null)
            {
                continue;
            }
            try
            {
                Logger.Trace($"Load {AssemblyName} from {path}");
                Assembly.LoadFrom(path);
            }
            catch (Exception exception)
            {
                Logger.Warning($"{AssemblyName} could not be loaded from {path}: {exception.Message}");
                return;
            }
            this.LoadNativeSni(directory);
            return;
        }
        // No runtime specific build declared anywhere - then the assembly next to the deps.json is the real one and
        // the normal assembly resolution of the generator is right.
        Logger.Trace($"No runtime specific {AssemblyName} found. The default assembly resolution is used.");
    }

    /// <summary>
    /// On Windows SqlClient talks to the network through a native library. It is loaded here and not on demand,
    /// because the first failed P/Invoke poisons the static constructor that triggered it for the rest of the
    /// process - there would be no second chance.
    /// </summary>
    /// <remarks>
    /// Not fatal: without the native library SqlClient falls back to its managed networking implementation, which
    /// serves a plain TCP connection just as well.
    /// </remarks>
    private void LoadNativeSni(string applicationDirectory)
    {
        try
        {
            this.nativeLibraryLoader.Load(applicationDirectory, NativeLibraryName, AssemblyName);
        }
        catch (Exception exception)
        {
            Logger.Trace($"Native SNI library could not be loaded ({exception.Message}). SqlClient uses its managed networking implementation instead.");
        }
    }

    /// <summary>
    /// The output directories of the users project - the ones holding its deps.json. They are not known directly,
    /// but every assembly loaded from the project sits in one of them.
    /// </summary>
    private IEnumerable<string> GetApplicationDirectories()
    {
        return this.environment.LoadedAssemblies
                   .Select(assembly => assembly.Location)
                   .Where(location => !string.IsNullOrEmpty(location))
                   .Select(FileSystem.GetDirectoryName)
                   .Where(directory => !string.IsNullOrEmpty(directory))
                   .Select(directory => directory!)
                   .Distinct();
    }
}
