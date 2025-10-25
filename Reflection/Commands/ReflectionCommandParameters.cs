using KY.Generator.Command;
using KY.Generator.Languages;

namespace KY.Generator;

public class ReflectionCommandParameters : GeneratorCommandParameters
{
    public string? Name { get; set; }
    public string? Namespace { get; set; }
    public bool OnlySubTypes { get; set; }
    public ILanguage? Language { get; set; }

    public static string[] Names { get; } = [..ToCommand(nameof(ReflectionCommandParameters))];

    public ReflectionCommandParameters()
        : base(Names.First())
    { }
}
