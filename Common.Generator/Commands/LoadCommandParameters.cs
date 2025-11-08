using KY.Generator.Command;

namespace KY.Generator.Commands;

public class LoadCommandParameters : GeneratorCommandParameters
{
    [GeneratorParameter("assembly")]
    public string? Assembly { get; set; }

    public string? From { get; set; }

    public static string[] Names { get; } = [..ToCommand(nameof(LoadCommand))];

    public LoadCommandParameters()
        : base(Names.First())
    { }
}
