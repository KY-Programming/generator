using KY.Generator.Command;

namespace KY.Generator;

public class AngularModelCommandParameters : GeneratorCommandParameters
{
    public bool? WithSignals { get; set; }

    public static string[] Names { get; } = [..ToCommand(nameof(AngularModelCommandParameters))];

    public AngularModelCommandParameters()
        : base(Names.First())
    { }
}
