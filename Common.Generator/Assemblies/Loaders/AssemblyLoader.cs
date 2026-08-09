using System.Diagnostics;
using System.Reflection;
using KY.Core;
using KY.Core.Dependency;
using KY.Core.Extension;

namespace KY.Generator;

public class AssemblyLoader
{
    private static bool isActivated;

    public List<IAssemblyLoader> Loaders { get; } = [];

    public EngineVersionGuard EngineVersionGuard { get; set; } = new();

    public List<string> IgnoredAssemblies { get; } =
    [
        "mscorlib",
        "netstandard"
    ];

    public AssemblyLoader(IDependencyResolver resolver)
    {
        this.Loaders.Add(resolver.Create<SystemAssemblyLoader>());
        this.Loaders.Add(resolver.Create<AssemblyLocatorsLoader>());
    }

    public void Activate()
    {
        if (isActivated)
        {
            return;
        }
        isActivated = true;
        AppDomain.CurrentDomain.AssemblyResolve += this.Resolve;
    }

    public void Deactivate()
    {
        AppDomain.CurrentDomain.AssemblyResolve -= this.Resolve;
        isActivated = false;
    }

    private Assembly? Resolve(object sender, ResolveEventArgs args)
    {
        return this.Load(AssemblyLocateInfo.From(args));
    }

    public Assembly? Load(AssemblyLocateInfo info)
    {
        if (this.IgnoredAssemblies.Contains(info.Name) || info.Name.EndsWith(".resources", StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }
        Assembly? assembly = GetLoadedAssembly(info);
        if (assembly != null)
        {
            return assembly;
        }
        Logger.Trace($"Try to load assembly {info}...");
        AssemblyLocation? location = this.Locate(info);
        return location == null ? null : this.Load(location, info);
    }

    public AssemblyMetaData? LoadMetaData(AssemblyLocateInfo info)
    {
        if (this.IgnoredAssemblies.Contains(info.Name))
        {
            return null;
        }
        Assembly? assembly = GetLoadedAssembly(info);
        if (assembly != null)
        {
            return AssemblyMetaData.From(assembly);
        }
        AssemblyLocation? location = this.Locate(info);
        return location == null ? null : AssemblyMetaData.From(location.Path);
    }

    private AssemblyLocation? Locate(AssemblyLocateInfo info)
    {
        foreach (IAssemblyLoader loader in this.Loaders)
        {
            AssemblyLocation? location = loader.Load(info);
            if (location != null)
            {
                return location;
            }
        }
        return null;
    }

    private Assembly Load(AssemblyLocation assemblyLocation, AssemblyLocateInfo info)
    {
        this.CheckEngineVersion(assemblyLocation, info);
        Stopwatch loadStopwatch = new();
        loadStopwatch.Start();
        try
        {
            return AssemblyLoadContext.Default.LoadFromAssemblyPath(assemblyLocation.Path);
        }
        catch (TargetInvocationException)
        {
            Logger.Trace("Could not load assembly. Trying to load dependencies first...");
            Assembly assembly = Assembly.LoadFile(assemblyLocation.Path);
            Logger.Trace($"All dependencies loaded. Clean up and try to load {info.Name} again...");
            AssemblyLoadContext.GetLoadContext(assembly).Unload();
            return AssemblyLoadContext.Default.LoadFromAssemblyPath(assemblyLocation.Path);
        }
        finally
        {
            loadStopwatch.Stop();
            Logger.Trace($"Assembly {info.Name} loaded in {loadStopwatch.FormattedElapsed()}");
        }
    }

    /// <summary>
    /// The locators fall back to the newest version they can find, so an engine assembly can silently resolve to a
    /// version that does not match the running engine. Reading the name off the file keeps the check in front of the
    /// load - once the assembly is in the process, the mismatch only shows up as an unrelated type load error.
    /// </summary>
    private void CheckEngineVersion(AssemblyLocation assemblyLocation, AssemblyLocateInfo info)
    {
        if (!EngineVersionGuard.IsEngineAssembly(info.Name))
        {
            return;
        }
        Version? foundVersion;
        try
        {
            foundVersion = AssemblyName.GetAssemblyName(assemblyLocation.Path).Version;
        }
        catch (Exception exception)
        {
            Logger.Trace($"Could not read the version of {assemblyLocation.Path} to compare it with the running engine. {exception.Message}");
            return;
        }
        this.EngineVersionGuard.Check(info.Name, info.Version, foundVersion, assemblyLocation.Path);
    }

    private static Assembly? GetLoadedAssembly(AssemblyLocateInfo info)
    {
        return AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.GetName().Name == info.Name);
    }
}
