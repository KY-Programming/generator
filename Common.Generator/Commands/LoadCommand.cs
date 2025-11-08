using System.Reflection;
using KY.Core;
using KY.Generator.Command;
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
        Logger.Trace("Execute load command...");
        LocateAssemblyResult result = GeneratorAssemblyLocator.Locate(this.Parameters.Assembly, this.environment.IsBeforeBuild);
        if (this.environment.IsBeforeBuild && !result.Success)
        {
            return this.SuccessAsync();
        }
        if (result.SwitchContext || !result.Success)
        {
            return this.ResultAsync(result);
        }
        this.environment.LoadedAssemblies.Add(result.Assembly);
        if (!string.IsNullOrEmpty(this.Parameters.From))
        {
            Assembly? fromAssembly = this.environment.LoadedAssemblies.FirstOrDefault(x => x.Location.Equals(this.Parameters.From, StringComparison.CurrentCultureIgnoreCase));
            if (fromAssembly != null)
            {
                GeneratorOptions fromOptions = this.options.Get<GeneratorOptions>(fromAssembly);
                this.options.Get(result.Assembly, fromOptions);
            }
        }
        GenerateWithLicenseAttribute? attribute = result.Assembly.GetCustomAttribute<GenerateWithLicenseAttribute>();
        if (attribute != null)
        {
            this.licenseService.Set(attribute.Certificate);
        }
        this.licenseService.Check();
        this.moduleLoader.LoadFromAttributesAndDirectReferences(result.Assembly);
        return this.SuccessAsync();
    }
}
