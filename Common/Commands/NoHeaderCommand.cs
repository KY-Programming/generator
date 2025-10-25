using KY.Generator.Command;

namespace KY.Generator.Commands;

public class NoHeaderCommand : GeneratorCommand<GeneratorCommandParameters>, IPrepareCommand
{
    private readonly Options options;
    public static string[] Names { get; } = [..ToCommand(nameof(NoHeaderCommand))];

    public NoHeaderCommand(Options options)
    {
        this.options = options;
    }

    public override IGeneratorCommandResult Run()
    {
        this.options.Get<GeneratorOptions>().AddHeader = false;
        return this.Success();
    }
}
