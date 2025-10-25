using KY.Core;
using KY.Generator.Command;
using KY.Generator.Models;

namespace KY.Generator.Commands;

internal class LoadCommand : GeneratorCommand<LoadCommandParameters>, IPrepareCommand
{
    private readonly GeneratorModuleLoader moduleLoader;
    private readonly IEnvironment environment;

    public LoadCommand(GeneratorModuleLoader moduleLoader, IEnvironment environment)
    {
        this.moduleLoader = moduleLoader;
        this.environment = environment;
    }

    public override IGeneratorCommandResult Run()
    {
        if (this.environment.LoadedAssemblies.Any(x => x.GetName().Name == this.Parameters.Assembly))
        {
            return this.Success();
        }
        Logger.Trace("Execute load command...");
        LocateAssemblyResult result = GeneratorAssemblyLocator.Locate(this.Parameters.Assembly, this.environment.IsBeforeBuild);
        if (this.environment.IsBeforeBuild && !result.Success)
        {
            return this.Success();
        }
        if (result.SwitchContext || !result.Success)
        {
            return result;
        }
        this.environment.LoadedAssemblies.Add(result.Assembly);
        this.moduleLoader.LoadFromAttributesAndDirectReferences(result.Assembly);
        return this.Success();
    }
}
