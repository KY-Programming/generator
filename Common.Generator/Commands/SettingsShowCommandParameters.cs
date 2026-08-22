using KY.Generator.Command;

namespace KY.Generator.Commands;

public class SettingsShowCommandParameters : GeneratorCommandParameters
{
    public static string[] Names { get; } = [..ToCommand(nameof(SettingsShowCommand)), "settings"];

    public SettingsShowCommandParameters()
        : base(Names.First())
    { }
}
