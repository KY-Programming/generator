using System.Diagnostics;
using KY.Core;
using KY.Core.Dependency;
using KY.Core.Extension;

namespace KY.Generator;

public class AssemblyLocatorsLoader : IAssemblyLoader
{
    public List<IAssemblyLocator> Locators { get; } = [];

    public AssemblyLocatorsLoader(IDependencyResolver resolver)
    {
        this.Locators.Add(resolver.Create<CachedAssemblyLocator>());
        this.Locators.Add(resolver.Create<LocalAssemblyLocator>());
        this.Locators.Add(resolver.Create<RuntimeLocator>());
        this.Locators.Add(resolver.Create<NugetAssemblyLocator>());
        this.Locators.Add(resolver.Create<LocalDeepSearchAssemblyLocator>());
    }

    public AssemblyLocation? Load(AssemblyLocateInfo info)
    {
        Stopwatch findStopwatch = new();
        findStopwatch.Start();
        AssemblyLocation? assemblyLocation = this.Locators.Select(locator => locator.Locate(info)).OfType<AssemblyLocation>().FirstOrDefault();
        findStopwatch.Stop();
        if (assemblyLocation == null)
        {
            Logger.Warning($"Assembly {info.Name} not found. Use GenerateWithAttribute to load additional assemblies.");
            return null;
        }
        Logger.Trace($"Assembly found here {assemblyLocation.Path} in {findStopwatch.FormattedElapsed()}");
        return assemblyLocation;
    }
}
