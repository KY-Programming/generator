using KY.Generator.Command;

namespace KY.Generator.Commands;

public class VersionCommandParameters : GeneratorCommandParameters
{
    [GeneratorParameter("d")]
    public bool ShowDetailed { get; set; }

    public static string[] Names { get; } = [..ToCommand(nameof(VersionCommand)), "v"];

    public VersionCommandParameters()
        : base(Names.First())
    { }
}
