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

    private static Assembly? GetLoadedAssembly(AssemblyLocateInfo info)
    {
        return AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.GetName().Name == info.Name);
    }
}
