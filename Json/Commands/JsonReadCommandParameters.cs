using KY.Generator.Command;

namespace KY.Generator.Commands;

public class JsonReadCommandParameters : GeneratorCommandParameters
{
    public static string[] Names { get; } = [..ToCommand(nameof(JsonReadCommandParameters))];

    public JsonReadCommandParameters()
        : base(Names.First())
    { }
}
