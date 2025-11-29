using System.Reflection;
using System.Runtime.Versioning;
using KY.Core;
using KY.Generator.Command;
using KY.Generator.Extensions;
using KY.Generator.Licensing;
using KY.Generator.Models;

namespace KY.Generator.Commands;

internal class LoadCommand : GeneratorCommand<LoadCommandParameters>, IPrepareCommand
{
    private readonly GeneratorModuleLoader moduleLoader;
    private readonly IEnvironment environment;
    private readonly LicenseService licenseService;
    private readonly Options options;

    public LoadCommand(GeneratorModuleLoader moduleLoader, IEnvironment environment, LicenseService licenseService, Options options)
    {
        this.moduleLoader = moduleLoader;
        this.environment = environment;
        this.licenseService = licenseService;
        this.options = options;
    }

    public override Task<IGeneratorCommandResult> Run()
    {
        if (this.environment.LoadedAssemblies.Any(x => x.GetName().Name == this.Parameters.Assembly))
        {
            return this.SuccessAsync();
        }
        IGeneratorCommandResult? alreadyLoadedResult = this.CheckAlreadyLoadedAssemblies();
        if (alreadyLoadedResult != null)
        {
            return this.ResultAsync(alreadyLoadedResult);
        }
        IGeneratorCommandResult? switchResult = this.CheckAssemblyCompatibility();
        if (switchResult != null)
        {
            return this.ResultAsync(switchResult);
        }
        return this.Load();
    }

    private IGeneratorCommandResult? CheckAlreadyLoadedAssemblies()
    {
        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies().Where(x => !x.IsDynamic))
        {
            if (assembly.Location.Equals(this.Parameters.Assembly, StringComparison.CurrentCultureIgnoreCase) || assembly.GetName().Name.Equals(this.Parameters.Assembly, StringComparison.CurrentCultureIgnoreCase))
            {
                return this.Success();
            }
        }
        return null;
    }

    private IGeneratorCommandResult? CheckAssemblyCompatibility()
    {
        if (this.Parameters.Assembly == null)
        {
            Logger.Error("Load command requires assembly parameter.");
            return this.Error();
        }

        Assembly? entryAssembly = Assembly.GetEntryAssembly();
        ProcessorArchitecture? entryArchitecture = entryAssembly?.GetName().ProcessorArchitecture;
        try
        {
            ProcessorArchitecture assemblyArchitecture = AssemblyName.GetAssemblyName(this.Parameters.Assembly).ProcessorArchitecture;
            if (assemblyArchitecture != ProcessorArchitecture.None)
            {
                bool isCompatible64 = (entryArchitecture == ProcessorArchitecture.Amd64 || entryArchitecture == ProcessorArchitecture.MSIL)
                                      && (assemblyArchitecture == ProcessorArchitecture.Amd64 || assemblyArchitecture == ProcessorArchitecture.MSIL);
                bool isCompatible86 = entryArchitecture == ProcessorArchitecture.X86 && assemblyArchitecture == ProcessorArchitecture.X86;
                if (!isCompatible64 && !isCompatible86)
                {
                    return this.SwitchContext(assemblyArchitecture);
                }
            }
        }
        catch (FileNotFoundException)
        {
            if (this.environment.IsBeforeBuild)
            {
                return this.Success();
            }
            throw;
        }
        catch (DirectoryNotFoundException)
        {
            if (this.environment.IsBeforeBuild)
            {
                return this.Success();
            }
            throw;
        }
        return null;
    }

    private Task<IGeneratorCommandResult> Load()
    {
        Logger.Trace("Execute load command...");
        try
        {
            SwitchableFramework? assemblyFramework = null;
            IList<CustomAttributeData> customAttributeData = AssemblyMetaData.From(this.Parameters.Assembly!).GetCustomAttributesData();
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
                    // Some unnecessary attributes cannot be read by an assembly with the wrong framework version, so ignore them
                }
            }
            assemblyFramework ??= SwitchableFramework.None;

            Assembly? entryAssembly = Assembly.GetEntryAssembly();
            SwitchableFramework? entryFramework = entryAssembly?.GetSwitchableFramework();
            if (entryFramework != assemblyFramework && assemblyFramework != SwitchableFramework.None)
            {
                return this.ResultAsync(this.SwitchContext(assemblyFramework.Value));
            }
            Assembly assembly = Assembly.LoadFrom(this.Parameters.Assembly!);
            this.environment.LoadedAssemblies.Add(assembly);
            this.ProcessFrom(assembly);
            this.ProcessLicense(assembly);
            this.moduleLoader.LoadFromAttributesAndDirectReferences(assembly);
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
        return this.SuccessAsync();
    }

    private void ProcessFrom(Assembly assembly)
    {
        if (!string.IsNullOrEmpty(this.Parameters.From))
        {
            Assembly? fromAssembly = this.environment.LoadedAssemblies.FirstOrDefault(x => x.Location.Equals(this.Parameters.From, StringComparison.CurrentCultureIgnoreCase));
            if (fromAssembly != null)
            {
                GeneratorOptions fromOptions = this.options.Get<GeneratorOptions>(fromAssembly);
                this.options.Get(assembly, fromOptions);
            }
        }
    }

    private void ProcessLicense(Assembly assembly)
    {
        GenerateWithLicenseAttribute? attribute = assembly.GetCustomAttribute<GenerateWithLicenseAttribute>();
        if (attribute != null)
        {
            this.licenseService.Set(attribute.Certificate);
        }
        this.licenseService.Check();
    }

    private static FrameworkName? TryParseFrameworkName(string? value)
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
