using System.Reflection;
using System.Runtime.Versioning;
using KY.Core;
using KY.Core.DataAccess;
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
        if (this.environment.LoadedAssemblies.Any(this.Matches))
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

    /// <summary>
    /// The assembly can already be part of the app domain, e.g. if a fluent generator assembly references the assembly
    /// it reads from. It still has to be registered, otherwise no type of it can be found later.
    /// </summary>
    private IGeneratorCommandResult? CheckAlreadyLoadedAssemblies()
    {
        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies().Where(x => !x.IsDynamic))
        {
            if (this.Matches(assembly))
            {
                this.Register(assembly);
                return this.Success();
            }
        }
        return null;
    }

    private bool Matches(Assembly assembly)
    {
        return assembly.Location.Equals(this.Parameters.Assembly, StringComparison.CurrentCultureIgnoreCase)
               || assembly.GetName().Name.Equals(this.Parameters.Assembly, StringComparison.CurrentCultureIgnoreCase);
    }

    private void Register(Assembly assembly)
    {
        this.environment.LoadedAssemblies.Add(assembly);
        this.ProcessFrom(assembly);
        this.ProcessLicense(assembly);
        this.ProcessRunAtSuccess(assembly);
        this.moduleLoader.LoadFromAttributesAndDirectReferences(assembly);
    }

    private IGeneratorCommandResult? CheckAssemblyCompatibility()
    {
        if (this.Parameters.Assembly == null)
        {
            Logger.Error("Load command requires assembly parameter.");
            return this.Error();
        }

        ProcessorArchitecture processArchitecture = AssemblyArchitectureReader.Current();
        try
        {
            ProcessorArchitecture assemblyArchitecture = AssemblyArchitectureReader.Read(this.Parameters.Assembly);
            if (!AssemblyArchitectureReader.IsCompatible(assemblyArchitecture, processArchitecture))
            {
                Logger.Trace($"Assembly {FileSystem.GetFileName(this.Parameters.Assembly)} is compiled for {assemblyArchitecture}, but the current process runs as {processArchitecture}");
                return this.SwitchContext(assemblyArchitecture);
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
            this.Register(assembly);
        }
        catch (EngineVersionMismatchException)
        {
            throw;
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
            Logger.Error($"Could not load assembly {this.Parameters.Assembly}\n{exception.Message}");
            return this.ErrorAsync();
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

    /// <summary>
    /// The attribute is read here and not by the annotation command, so it also reaches a run that generates
    /// nothing from annotations - a fluent one, or the background run of a project whose types are all handled.
    /// </summary>
    private void ProcessRunAtSuccess(Assembly assembly)
    {
        foreach (RunAtSuccessAttribute attribute in assembly.GetCustomAttributes<RunAtSuccessAttribute>())
        {
            if (!string.IsNullOrEmpty(attribute.Command) && !this.environment.RunAtSuccess.Contains(attribute.Command))
            {
                this.environment.RunAtSuccess.Add(attribute.Command);
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
