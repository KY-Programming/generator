using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using KY.Core;
using KY.Core.Nuget;
using KY.Generator.Extensions;
using KY.Generator.Models;

namespace KY.Generator
{
    public static class GeneratorAssemblyLocator
    {
        public static Assembly Locate(string assemblyName, GeneratorEnvironment environment, params SearchLocation[] locations)
        {
            NugetAssemblyLocator locator = NugetPackageDependencyLoader.CreateLocator();
            locator.Locations.InsertRange(0, locations);
            Version defaultVersion = typeof(CoreModule).Assembly.GetName().Version;

            Assembly entryAssembly = Assembly.GetEntryAssembly();
            ProcessorArchitecture entryArchitecture = entryAssembly.GetName().ProcessorArchitecture;
            ProcessorArchitecture assemblyArchitecture = AssemblyName.GetAssemblyName(assemblyName).ProcessorArchitecture;
            if (entryArchitecture != assemblyArchitecture)
            {
                environment.SwitchContext = true;
                environment.SwitchToArchitecture = assemblyArchitecture;
                return null;
            }

            try
            {
                SwitchableFramework entryFramework = entryAssembly.GetSwitchableFramework();
                SwitchableFramework? assemblyFramework = null;
                string[] frameworkFiles = Directory.GetFiles(RuntimeEnvironment.GetRuntimeDirectory(), "*.dll");
                PathAssemblyResolver resolver = new PathAssemblyResolver(AppDomain.CurrentDomain.GetAssemblies().Select(x => x.Location).Concat(frameworkFiles));
                MetadataLoadContext metadataLoadContext = new MetadataLoadContext(resolver);

                using (metadataLoadContext)
                {
                    Assembly assemblyData = metadataLoadContext.LoadFromAssemblyPath(assemblyName);
                    IList<CustomAttributeData> customAttributeData = assemblyData.GetCustomAttributesData();

                    foreach (CustomAttributeData attributeData in customAttributeData)
                    {
                        try
                        {
                            assemblyFramework = assemblyFramework ?? attributeData.ConstructorArguments.Select(x => x.Value as string)
                                                                                  .Where(x => x != null)
                                                                                  .Select(TryParseFrameworkName)
                                                                                  .FirstOrDefault()?
                                                                                  .GetSwitchableFramework();
                        }
                        catch
                        {
                            // Some unnecessary attributes can not be read by a assembly with the wrong framework version, so ignore them
                        }
                    }
                }

                if (entryFramework != assemblyFramework && assemblyFramework != SwitchableFramework.None)
                {
                    environment.SwitchContext = true;
                    environment.SwitchToFramework = assemblyFramework ?? SwitchableFramework.None;
                    return null;
                }
            }
            catch (TypeLoadException exception)
            {
                Logger.Warning($"Could not check framework compatibility, because {exception.TypeName} could not be loaded\n{exception.Message}");
            }
            catch (FileNotFoundException exception)
            {
                Logger.Warning($"Could not check framework compatibility, because an assembly could not be found\n{exception.Message}");
            }
            catch (Exception exception)
            {
                Logger.Warning($"Could not check framework compatibility, because an error occurred\n{exception.Message}");
            }
            return locator.Locate(assemblyName, defaultVersion);
        }

        private static FrameworkName TryParseFrameworkName(string x)
        {
            try
            {
                return new FrameworkName(x);
            }
            catch
            {
                return null;
            }
        }
    }
}