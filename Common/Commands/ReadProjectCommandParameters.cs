using KY.Generator.Command;

namespace KY.Generator.Commands;

public class ReadProjectCommandParameters : GeneratorCommandParameters
{
    public string? Solution { get; set; }
    public string? Project { get; set; }

    public static string[] Names { get; } = [..ToCommand(nameof(ReadProjectCommand)), "readid"];

    public ReadProjectCommandParameters()
        : base(Names.First())
    { }
}
