using KY.Generator.Command;

namespace KY.Generator.Commands;

public class SettingsSetCommandParameters : GeneratorCommandParameters
{
    /// <summary>
    /// The option to write, e.g. <c>options.addHeader</c> or <c>typescript.noIndex</c>
    /// </summary>
    public string? Key { get; set; }

    /// <summary>
    /// The value to write. Written as json, so <c>false</c> becomes a boolean and <c>Output/Models</c> a string
    /// </summary>
    public string? Value { get; set; }

    /// <summary>
    /// Writes to the settings of this machine instead of the one in the directory tree
    /// </summary>
    public bool Global { get; set; }

    /// <summary>
    /// The directory of the settings file to write. Defaults to the current directory
    /// </summary>
    public string? Path { get; set; }

    public static string[] Names { get; } = [..ToCommand(nameof(SettingsSetCommand))];

    public SettingsSetCommandParameters()
        : base(Names.First())
    { }
}
