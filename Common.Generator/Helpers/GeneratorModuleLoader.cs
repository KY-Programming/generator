using System.Diagnostics;
using System.Reflection;
using KY.Core;
using KY.Core.Dependency;
using KY.Core.Extension;
using KY.Core.Module;
using KY.Generator.Statistics;

namespace KY.Generator;

public class GeneratorModuleLoader
{
    private readonly ModuleFinder moduleFinder;
    private readonly StatisticsService statisticsService;
    private readonly IDependencyResolver resolver;
    private readonly AssemblyLoader assemblyLoader;
    private readonly List<ModuleBase> initializedModules = [];

    public GeneratorModuleLoader(ModuleFinder moduleFinder, StatisticsService statisticsService, IDependencyResolver resolver, AssemblyLoader assemblyLoader)
    {
        this.moduleFinder = moduleFinder;
        this.statisticsService = statisticsService;
        this.resolver = resolver;
        this.assemblyLoader = assemblyLoader;
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
        List<string> generatorAttributeTypeNames = [typeof(GenerateWithAttribute).FullName, typeof(GeneratePluginAttribute).FullName];
        AssemblyName[] referencedAssemblyNames = assembly.GetReferencedAssemblies();
        foreach (AssemblyName referencedAssemblyName in referencedAssemblyNames)
        {
            AssemblyMetaData? metaData = this.assemblyLoader.LoadMetaData(AssemblyLocateInfo.From(referencedAssemblyName));
            if (metaData == null)
            {
                continue;
            }
            IList<CustomAttributeData> customAttributesData = metaData.GetCustomAttributesData();
            bool hasGenerateAttribute = customAttributesData.Any(x => generatorAttributeTypeNames.Contains(x.AttributeType.FullName));
            if (!hasGenerateAttribute)
            {
                continue;
            }
            Assembly? referencedAssembly = this.assemblyLoader.Load(AssemblyLocateInfo.From(metaData));
            if (referencedAssembly == null)
            {
                continue;
            }
            this.LoadFromAttributesAndDirectReferences(referencedAssembly);
        }
        this.InitializeModules();
    }

    public void LoadFromAttributes(Assembly assembly)
    {
        IEnumerable<GenerateWithAttribute> generateWithAttributes = assembly.GetCustomAttributes<GenerateWithAttribute>();
        foreach (GenerateWithAttribute generateWithAttribute in generateWithAttributes)
        {
            Version? version = null;
            if (generateWithAttribute.Version != null)
            {
                version = new Version(generateWithAttribute.Version);
            }
            if (generateWithAttribute.UseSameVersion)
            {
                version = assembly.GetName().Version;
            }
            if (generateWithAttribute.UseGeneratorVersion)
            {
                version = typeof(CoreModule).Assembly.GetName().Version;
            }
            Assembly? generatorAssembly = this.assemblyLoader.Load(new AssemblyLocateInfo(generateWithAttribute.AssemblyName, version, hint: generateWithAttribute.AssemblyPath));
            if (generatorAssembly == null)
            {
                continue;
            }
            this.Load(generatorAssembly);
        }
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
            Logger.Trace($"{module.GetType().Name.Replace("Module", "")}-{module.GetType().Assembly.GetName().Version} module loaded in {stopwatch.FormattedElapsed()}");
            this.initializedModules.Add(module);
        }
    }
}
