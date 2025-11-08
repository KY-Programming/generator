using KY.Generator.Command;

namespace KY.Generator.Commands;

public class BeforeBuildCommandParameters : GeneratorCommandParameters
{
    public static string[] Names { get; } = [..ToCommand(nameof(BeforeBuildCommand))];

    public BeforeBuildCommandParameters()
        : base(Names.First())
    { }
}
