using KY.Generator.Command;

namespace KY.Generator.Commands;

public class CleanupCommandParameters : GeneratorCommandParameters
{
    public bool Logs { get; set; } = true;
    public bool Statistics { get; set; } = true;

    public static string[] Names { get; } = [..ToCommand(nameof(CleanupCommand))];

    public CleanupCommandParameters()
        : base(Names.First())
    { }
}
