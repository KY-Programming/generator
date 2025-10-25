using KY.Generator.Command;

namespace KY.Generator.Commands;

internal class NoHeaderCommand : GeneratorCommand<NoHeaderCommandParameters>, IPrepareCommand
{
    private readonly Options options;

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
