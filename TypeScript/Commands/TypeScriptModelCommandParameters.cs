using KY.Generator.Command;

namespace KY.Generator;

public class TypeScriptModelCommandParameters : GeneratorCommandParameters
{
    public static string[] Names { get; } = [..ToCommand(nameof(TypeScriptModelCommandParameters)), "typescript-model", "ts-model"];

    public TypeScriptModelCommandParameters()
        : base(Names.First())
    { }
}
