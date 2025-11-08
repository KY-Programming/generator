using KY.Generator.Command;

namespace KY.Generator.Commands;

public class FluentCommandParameters : GeneratorCommandParameters
{
    public static string[] Names { get; } = [..ToCommand(nameof(FluentCommand))];

    public FluentCommandParameters()
        : base(Names.First())
    { }
}
