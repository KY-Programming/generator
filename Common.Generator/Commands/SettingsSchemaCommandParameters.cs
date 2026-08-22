using KY.Generator.Command;

namespace KY.Generator.Commands;

public class SettingsSchemaCommandParameters : GeneratorCommandParameters
{
    /// <summary>
    /// The file to write the schema to. Defaults to <c>schema.json</c> in the current directory
    /// </summary>
    public string? Output { get; set; }

    public static string[] Names { get; } = [..ToCommand(nameof(SettingsSchemaCommand))];

    public SettingsSchemaCommandParameters()
        : base(Names.First())
    { }
}
