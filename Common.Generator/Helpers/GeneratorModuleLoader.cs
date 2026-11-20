using System.Diagnostics;
using System.Reflection;
using KY.Core;
using KY.Core.DataAccess;
using KY.Core.Dependency;
using KY.Core.Extension;
using KY.Core.Module;
using KY.Generator.Statistics;

namespace KY.Generator;

public class GeneratorModuleLoader
{
    private readonly StatisticsService statisticsService;
    private readonly IDependencyResolver resolver;
    private readonly AssemblyLoader assemblyLoader;
    private readonly List<ModuleBase> initializedModules = [];
    private ModuleFinder? moduleFinder;
    private bool isCreatingModuleFinder;

    public GeneratorModuleLoader(StatisticsService statisticsService, IDependencyResolver resolver, AssemblyLoader assemblyLoader)
    {
        this.statisticsService = statisticsService;
        this.resolver = resolver;
        this.assemblyLoader = assemblyLoader;
    }

    /// <summary>
    /// Created on first use instead of being injected. The constructor of <see cref="ModuleFinder"/> already creates
    /// the modules of every assembly the process has loaded - none on the first generator of a process, all of them
    /// on the next. A module asks for this loader in its constructor (<c>DependsOn</c>), so with the finder injected
    /// the resolver would start a second finder while the first one is still being built, which creates the modules
    /// again, which ask again... - every command registered twice and a stack overflow at the end
    /// </summary>
    private ModuleFinder ModuleFinder
    {
        get
        {
            if (this.moduleFinder == null)
            {
                this.isCreatingModuleFinder = true;
                try
                {
                    this.moduleFinder = this.resolver.Get<ModuleFinder>();
                }
                finally
                {
                    this.isCreatingModuleFinder = false;
                }
                // Picks up what a module constructor loaded after the finder took its list of loaded assemblies
                this.moduleFinder.LoadFromAssemblies();
            }
            return this.moduleFinder;
        }
    }

    public void Load(string path, string moduleFileNameSearchPattern)
    {
        this.InitializeModules(this.ModuleFinder.LoadFrom(path, moduleFileNameSearchPattern));
    }

    public void Load(Assembly assembly)
    {
        if (this.isCreatingModuleFinder)
        {
            // Asked for by a module the finder creates in its constructor. The finder goes through every loaded
            // assembly, the requested one included, and the modules are initialized once it is done
            return;
        }
        this.InitializeModules(this.ModuleFinder.LoadFrom(assembly));
    }

    /// <summary>
    /// Loads every module that ships with the tool, instead of only the ones the generated project references. Used
    /// by the commands that describe what the generator can do - the answer must not depend on which project the
    /// command happens to run in
    /// </summary>
    public void LoadShipped()
    {
        // The modules sit next to the executable in the packaged tool, and in the shared netstandard2.0 folder in
        // a build of this repository
        foreach (string path in new[] { AppContext.BaseDirectory, FileSystem.Combine(FileSystem.Parent(FileSystem.FormatPath(AppContext.BaseDirectory)), "netstandard2.0") })
        {
            if (FileSystem.DirectoryExists(path))
            {
                this.Load(path, "KY.Generator.*.dll");
            }
        }
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
        List<ModuleBase> list = (modules ?? this.ModuleFinder.Modules).ToList();
        list.RemoveAll(x => this.initializedModules.Contains(x));
        Dictionary<ModuleBase, Stopwatch> stopwatches = list.ToDictionary(x => x, _ => new Stopwatch());
        this.statisticsService.Data.InitializedModules = this.ModuleFinder.Modules.Count;
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
        if (list.Count > 0)
        {
            // A module brings its own section of the configuration file with it, and the modules are loaded while
            // the commands run. Without this the section of a module would be read too late to have any effect
            this.resolver.TryGet<SettingsService>()?.Apply();
        }
    }
}
