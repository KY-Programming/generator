using KY.Generator.Command;

namespace KY.Generator.Commands;

public class OptionsCommandParameters : GeneratorCommandParameters
{
    public string? Statistics { get; set; }
    public string? Output { get; set; }

    public static string[] Names { get; } = [..ToCommand(nameof(OptionsCommand)), "set"];

    public OptionsCommandParameters()
        : base(Names.First())
    { }
}
