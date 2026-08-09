using System.Reflection;
using System.Runtime.InteropServices;
using KY.Core;

namespace KY.Generator;

/// <summary>
/// Loads a native library that belongs to the users project instead of the generator itself. Has to be called before
/// the first P/Invoke into that library happens - a failed one poisons the static constructor that triggered it for
/// the rest of the process.
/// </summary>
public class NativeLibraryLoader
{
    private static readonly Dictionary<string, IntPtr> loadedLibraries = new();
    private readonly NativeAssetLocator locator;

    public NativeLibraryLoader(NativeAssetLocator locator)
    {
        this.locator = locator;
    }

    /// <param name="applicationDirectory">Output directory of the users project - the one containing its deps.json.</param>
    /// <param name="libraryName">Name as used in the DllImport, without prefix and extension (e.g. e_sqlite3).</param>
    /// <param name="pinvokeAssemblyName">Assembly declaring the DllImport. Not necessarily the assembly used by the
    /// generator - Microsoft.Data.Sqlite for example delegates to SQLitePCLRaw.provider.e_sqlite3.</param>
    public void Load(string applicationDirectory, string libraryName, string? pinvokeAssemblyName = null)
    {
        if (loadedLibraries.ContainsKey(libraryName))
        {
            return;
        }
        string path = this.locator.Locate(applicationDirectory, libraryName)
                      ?? throw new InvalidOperationException($"Native library '{libraryName}' for runtime '{NativeAssetLocator.RuntimeIdentifiers.First()}' not found. Searched the deps.json in '{applicationDirectory}' and the NuGet cache. Ensure the project provides this library for the platform the generator runs on.");
        Logger.Trace($"Load native library {libraryName} from {path}");
        IntPtr handle = LoadLibrary(path, libraryName);
        loadedLibraries[libraryName] = handle;
        RegisterResolver(libraryName, pinvokeAssemblyName, handle);
    }

#if NETSTANDARD2_0
    [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern IntPtr LoadLibraryW(string path);

    private static IntPtr LoadLibrary(string path, string libraryName)
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            throw new InvalidOperationException($"Loading the native library '{libraryName}' on this platform requires .NET Core 3.0 or newer. Run the generator on a newer runtime.");
        }
        IntPtr handle = LoadLibraryW(path);
        if (handle == IntPtr.Zero)
        {
            throw new InvalidOperationException($"Native library '{libraryName}' could not be loaded from '{path}'. Error code: {Marshal.GetLastWin32Error()}");
        }
        return handle;
    }

    private static void RegisterResolver(string libraryName, string? pinvokeAssemblyName, IntPtr handle)
    {
        // The Windows loader answers the LoadLibrary of the following P/Invoke with the module loaded above, because
        // it matches by file name. NativeLibrary, which would make this explicit, requires .NET Core 3.0 or newer.
    }
#else
    private static IntPtr LoadLibrary(string path, string libraryName)
    {
        return NativeLibrary.Load(path);
    }

    private static void RegisterResolver(string libraryName, string? pinvokeAssemblyName, IntPtr handle)
    {
        if (pinvokeAssemblyName == null)
        {
            return;
        }
        try
        {
            // Loading the library alone is enough on most platforms, the resolver makes it deterministic
            Assembly assembly = Assembly.Load(new AssemblyName(pinvokeAssemblyName));
            // Some providers spell the extension out in their DllImport (e.g. "Microsoft.Data.SqlClient.SNI.dll"),
            // others do not (e.g. "e_sqlite3") - both have to match the name the library was located under.
            NativeLibrary.SetDllImportResolver(assembly, (name, _, _) => IsSameLibrary(name, libraryName) ? handle : IntPtr.Zero);
        }
        catch (Exception exception)
        {
            Logger.Trace($"Could not register a resolver for {libraryName} on {pinvokeAssemblyName}: {exception.Message}. The already loaded library is used instead.");
        }
    }

    private static bool IsSameLibrary(string requested, string libraryName)
    {
        return TrimExtension(requested).Equals(TrimExtension(libraryName), StringComparison.OrdinalIgnoreCase);
    }

    private static string TrimExtension(string name)
    {
        return name.EndsWith(".dll", StringComparison.OrdinalIgnoreCase) ? name.Substring(0, name.Length - 4) : name;
    }
#endif
}
