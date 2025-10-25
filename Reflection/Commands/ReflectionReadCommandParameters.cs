using KY.Generator.Command;

namespace KY.Generator;

public class ReflectionReadCommandParameters : GeneratorCommandParameters
{
    public string? Namespace { get; set; }
    public string? Name { get; set; }
    public bool OnlySubTypes { get; set; }

    public static string[] Names { get; } = [..ToCommand(nameof(ReflectionReadCommandParameters))];

    public ReflectionReadCommandParameters()
        : base(Names.First())
    { }
}
