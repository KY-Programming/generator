using KY.Generator.Command;

namespace KY.Generator.TypeScript.Commands;

public class TypeScriptModelCommandParameters : GeneratorCommandParameters
{
    public static string[] Names { get; } = [..ToCommand(nameof(TypeScriptModelCommand)), "typescript-model", "ts-model"];

    public TypeScriptModelCommandParameters()
        : base(Names.First())
    { }
}
