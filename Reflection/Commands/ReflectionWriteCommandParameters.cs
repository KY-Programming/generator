using KY.Generator.Command;

namespace KY.Generator;

public class ReflectionWriteCommandParameters : GeneratorCommandParameters
{
    public string? Name { get; set; }
    public string? Namespace { get; set; }

    public static string[] Names { get; } = [..ToCommand(nameof(ReflectionWriteCommandParameters))];

    public ReflectionWriteCommandParameters()
        : base(Names.First())
    { }
}
