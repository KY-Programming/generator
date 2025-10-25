using KY.Generator.Command;

namespace KY.Generator.Commands;

public class JsonWriteCommandParameters : GeneratorCommandParameters
{
    public static string[] Names { get; } = [..ToCommand(nameof(JsonWriteCommandParameters))];

    public string? ModelName { get; set; }
    public string? ModelNamespace { get; set; }
    public bool WithReader { get; set; }

    public JsonWriteCommandParameters()
        : base(Names.First())
    {
        this.FieldsToProperties = true;
        this.PropertiesToFields = false;
        this.WithReader = true;
    }
}
