using KY.Generator.Command;

namespace KY.Generator.Commands;

public class SettingsInitCommandParameters : GeneratorCommandParameters
{
    /// <summary>
    /// Writes the settings of this machine instead of one in the directory tree
    /// </summary>
    public bool Global { get; set; }

    /// <summary>
    /// Overwrites an existing settings file
    /// </summary>
    public bool Force { get; set; }

    /// <summary>
    /// The directory to write the settings file to. Defaults to the current directory
    /// </summary>
    public string? Path { get; set; }

    public static string[] Names { get; } = [..ToCommand(nameof(SettingsInitCommand))];

    public SettingsInitCommandParameters()
        : base(Names.First())
    { }
}
