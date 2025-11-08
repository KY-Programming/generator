using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using KY.Core;
using KY.Core.DataAccess;
using KY.Core.Nuget;
using KY.Generator.Command;
using KY.Generator.Extensions;
using KY.Generator.Models;

namespace KY.Generator;

public static class GeneratorAssemblyLocator
{
    public static LocateAssemblyResult Locate(string assemblyPath, bool isBeforeBuild, params SearchLocation[] locations)
    {
        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies().Where(x => !x.IsDynamic))
        {
            if (assembly.Location.Equals(assemblyPath, StringComparison.CurrentCultureIgnoreCase) || assembly.GetName().Name.Equals(assemblyPath, StringComparison.CurrentCultureIgnoreCase))
            {
                return new LocateAssemblyResult(assembly);
            }
        }

        Assembly entryAssembly = Assembly.GetEntryAssembly();
        ProcessorArchitecture? entryArchitecture = entryAssembly?.GetName().ProcessorArchitecture;
        try
        {
            ProcessorArchitecture assemblyArchitecture = AssemblyName.GetAssemblyName(assemblyPath).ProcessorArchitecture;
            if (assemblyArchitecture != ProcessorArchitecture.None)
            {
                bool isCompatible64 = (entryArchitecture == ProcessorArchitecture.Amd64 || entryArchitecture == ProcessorArchitecture.MSIL)
                                      && (assemblyArchitecture == ProcessorArchitecture.Amd64 || assemblyArchitecture == ProcessorArchitecture.MSIL);
                bool isCompatible86 = entryArchitecture == ProcessorArchitecture.X86 && assemblyArchitecture == ProcessorArchitecture.X86;
                if (!isCompatible64 && !isCompatible86)
                {
                    return new LocateAssemblyResult(assemblyArchitecture);
                }
            }
        }
        catch (FileNotFoundException)
        {
            if (isBeforeBuild)
            {
                return new LocateAssemblyResult();
            }
            throw;
        }
        catch (DirectoryNotFoundException)
        {
            if (isBeforeBuild)
            {
                return new LocateAssemblyResult();
            }
            throw;
        }

        try
        {
            SwitchableFramework? assemblyFramework = null;
            string[] frameworkFiles = FileSystem.GetFiles(RuntimeEnvironment.GetRuntimeDirectory(), "*.dll");
            IEnumerable<string> loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies()
                                                            .Where(x => !x.IsDynamic)
                                                            .Select(x => x.Location)
                                                            .Where(x => !string.IsNullOrEmpty(x));
            PathAssemblyResolver resolver = new(loadedAssemblies.Concat(frameworkFiles));
            MetadataLoadContext metadataLoadContext = new(resolver);

            using (metadataLoadContext)
            {
                Assembly assemblyData = metadataLoadContext.LoadFromAssemblyPath(assemblyPath);
                IList<CustomAttributeData> customAttributeData = assemblyData.GetCustomAttributesData();

                foreach (CustomAttributeData attributeData in customAttributeData)
                {
                    try
                    {
                        assemblyFramework ??= attributeData.ConstructorArguments.Select(x => x.Value as string)
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
                assemblyFramework ??= SwitchableFramework.None;
            }

            SwitchableFramework entryFramework = entryAssembly.GetSwitchableFramework();
            if (entryFramework != assemblyFramework && assemblyFramework != SwitchableFramework.None)
            {
                return new LocateAssemblyResult(assemblyFramework.Value);
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
        NugetAssemblyLocator locator = NugetPackageDependencyLoader.CreateLocator();
        locator.Locations.InsertRange(0, locations);
        Version defaultVersion = typeof(CoreModule).Assembly.GetName().Version;
        return new LocateAssemblyResult(locator.Locate(assemblyPath, defaultVersion));
    }

    private static FrameworkName TryParseFrameworkName(string value)
    {
        if (string.IsNullOrEmpty(value) || !value.Contains(','))
        {
            return null;
        }
        try
        {
            return new FrameworkName(value);
        }
        catch
        {
            return null;
        }
    }
}

public class LocateAssemblyResult : IGeneratorCommandResult
{
    public Assembly Assembly { get; }
    public bool Success { get; }
    public bool SwitchContext { get; }
    public ProcessorArchitecture? SwitchToArchitecture { get; }
    public SwitchableFramework SwitchToFramework { get; }
    public bool SwitchToAsync => false;
    public bool RerunOnAsync => false;

    public LocateAssemblyResult()
    {
        this.Success = false;
    }

    public LocateAssemblyResult(Assembly assembly)
    {
        this.Assembly = assembly;
        this.Success = assembly != null;
    }

    public LocateAssemblyResult(ProcessorArchitecture processorArchitecture)
    {
        this.Success = false;
        this.SwitchContext = true;
        this.SwitchToArchitecture = processorArchitecture;
    }

    public LocateAssemblyResult(SwitchableFramework framework)
    {
        this.Success = false;
        this.SwitchContext = true;
        this.SwitchToFramework = framework;
    }
}
