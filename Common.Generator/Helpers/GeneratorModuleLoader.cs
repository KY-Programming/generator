using System.Diagnostics;
using System.Reflection;
using KY.Core;
using KY.Core.DataAccess;
using KY.Core.Dependency;
using KY.Core.Module;
using KY.Core.Nuget;
using KY.Generator.Statistics;

namespace KY.Generator;

public class GeneratorModuleLoader
{
    private readonly ModuleFinder moduleFinder;
    private readonly StatisticsService statisticsService;
    private readonly IDependencyResolver resolver;
    private readonly List<ModuleBase> initializedModules = [];

    public GeneratorModuleLoader(ModuleFinder moduleFinder, StatisticsService statisticsService, IDependencyResolver resolver)
    {
        this.moduleFinder = moduleFinder;
        this.statisticsService = statisticsService;
        this.resolver = resolver;
    }

    public void Load(string path, string moduleFileNameSearchPattern)
    {
        this.InitializeModules(this.moduleFinder.LoadFrom(path, moduleFileNameSearchPattern));
    }

    public void Load(Assembly assembly)
    {
        this.InitializeModules(this.moduleFinder.LoadFrom(assembly));
    }

    public void LoadFromAttributesAndDirectReferences(Assembly assembly)
    {
        this.LoadFromAttributes(assembly);
        AssemblyName[] referencedAssemblyNames = assembly.GetReferencedAssemblies();
        foreach (AssemblyName referencedAssemblyName in referencedAssemblyNames)
        {
            string lowerAssemblyName = referencedAssemblyName.Name.ToLowerInvariant();
            if (!lowerAssemblyName.Contains("generator") && !lowerAssemblyName.Contains("annotation") && !lowerAssemblyName.Contains("plugin"))
            {
                continue;
            }
            NugetAssemblyLocator locator = NugetPackageDependencyLoader.CreateLocator();
            locator.Locations.Insert(0, new SearchLocation(assembly.Location).SearchOnlyLocal());
            Version defaultVersion = typeof(CoreModule).Assembly.GetName().Version;
            Assembly? referencedAssembly = locator.Locate(referencedAssemblyName.FullName, defaultVersion);
            if (referencedAssembly == null)
            {
                continue;
            }
            this.LoadFromAttributes(referencedAssembly);
        }
        this.InitializeModules();
    }

    public bool LoadFromAttributes(Assembly assembly)
    {
        bool result = false;
        IEnumerable<GenerateWithAttribute> generateWithAttributes = assembly.GetCustomAttributes<GenerateWithAttribute>();
        foreach (GenerateWithAttribute generateWithAttribute in generateWithAttributes)
        {
            NugetAssemblyLocator locator = NugetPackageDependencyLoader.CreateLocator();
            if (generateWithAttribute?.AssemblyPath != null)
            {
                string path = FileSystem.Combine(assembly.Location, generateWithAttribute.AssemblyPath);
                locator.Locations.Insert(0, new SearchLocation(path));
                if (path.Contains("\\lib\\"))
                {
                    locator.Locations.Insert(1, new SearchLocation(path.Replace("\\lib\\", "\\ref\\")));
                }
            }
            if (generateWithAttribute?.AssemblyName == null)
            {
                continue;
            }
            locator.Locations.Insert(0, new SearchLocation(assembly.Location).SearchOnlyLocal());
            Version defaultVersion = typeof(CoreModule).Assembly.GetName().Version;
            Assembly? generatorAssembly = locator.Locate(generateWithAttribute.AssemblyName, defaultVersion);
            if (generatorAssembly == null)
            {
                continue;
            }
            this.Load(generatorAssembly);
            result = true;
        }
        return result;
    }

    public void InitializeModules(IEnumerable<ModuleBase>? modules = null)
    {
        List<ModuleBase> list = (modules ?? this.moduleFinder.Modules).ToList();
        list.RemoveAll(x => this.initializedModules.Contains(x));
        Dictionary<ModuleBase, Stopwatch> stopwatches = list.ToDictionary(x => x, _ => new Stopwatch());
        this.statisticsService.Data.InitializedModules = this.moduleFinder.Modules.Count;
        foreach (ModuleBase module in list)
        {
            stopwatches[module].Start();
            this.resolver.Bind<ModuleBase>().To(module);
            stopwatches[module].Stop();
        }
        foreach (ModuleBase module in list)
        {
            Stopwatch stopwatch = stopwatches[module];
            stopwatch.Start();
            module.Initialize();
            stopwatch.Stop();
            Logger.Trace($"{module.GetType().Name.Replace("Module", "")}-{module.GetType().Assembly.GetName().Version} module loaded in {(stopwatch.ElapsedMilliseconds >= 1 ? stopwatch.ElapsedMilliseconds.ToString() : "<1")} ms");
            this.initializedModules.Add(module);
        }
    }
}
