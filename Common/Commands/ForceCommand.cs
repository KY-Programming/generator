using KY.Generator.Command;
using KY.Generator.Models;

namespace KY.Generator.Commands;

internal class ForceCommand : GeneratorCommand<ForceCommandParameters>, IPrepareCommand
{
    private readonly IEnvironment environment;

    public ForceCommand(IEnvironment environment)
    {
        this.environment = environment;
    }

    public override IGeneratorCommandResult Run()
    {
        this.environment.Force = true;
        return this.Success();
    }
}
