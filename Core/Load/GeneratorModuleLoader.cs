using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using KY.Core;
using KY.Core.DataAccess;
using KY.Core.Dependency;
using KY.Core.Module;

namespace KY.Generator.Load
{
    internal class GeneratorModuleLoader
    {
        private readonly IDependencyResolver resolver;
        private readonly ModuleFinder moduleFinder;

        public GeneratorModuleLoader(IDependencyResolver resolver, ModuleFinder moduleFinder)
        {
            this.resolver = resolver;
            this.moduleFinder = moduleFinder;
        }

        public void Load(IEnumerable<string> modules)
        {
            string assemblyRoot = FileSystem.Parent(Assembly.GetEntryAssembly()?.CodeBase.TrimStart("file:///"));
            string currentDirectory = Environment.CurrentDirectory;
            List<ModuleBase> loadedModules = this.moduleFinder.Modules.ToList();
            foreach (string module in modules)
            {
                List<string> searchLocations = new List<string>();
                searchLocations.Add(module);
                searchLocations.Add(FileSystem.Combine(assemblyRoot, module));
                searchLocations.Add(FileSystem.Combine(currentDirectory, module));

                int oldModuleCount = this.moduleFinder.Modules.Count;
                foreach (string searchLocation in searchLocations)
                {
                    this.moduleFinder.LoadFrom(searchLocation);
                    if (oldModuleCount != this.moduleFinder.Modules.Count)
                    {
                        break;
                    }
                }
                if (oldModuleCount == this.moduleFinder.Modules.Count)
                {
                    Logger.Trace($"Assembly not found or no module in assembly {module}. Searched in:");
                    searchLocations.ForEach(location => Logger.Trace($"  - {location}"));
                }
            }
            List<ModuleBase> newModules = this.moduleFinder.Modules.Where(module => !loadedModules.Contains(module)).ToList();
            foreach (ModuleBase module in newModules)
            {
                Logger.Trace($"{module.GetType().Name.Replace("Module", "")}-{module.GetType().Assembly.GetName().Version} module loaded");
            }
            newModules.ForEach(module => this.resolver.Bind<ModuleBase>().To(module));
            newModules.ForEach(module => module.Initialize());
        }
    }
}