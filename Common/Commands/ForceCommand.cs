using KY.Generator.Command;
using KY.Generator.Models;

namespace KY.Generator.Commands;

public class ForceCommand : GeneratorCommand<GeneratorCommandParameters>, IPrepareCommand
{
    private readonly IEnvironment environment;
    public static string[] Names { get; } = [..ToCommand(nameof(ForceCommand))];

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
