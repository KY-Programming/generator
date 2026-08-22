using KY.Generator.Command;

namespace KY.Generator.Commands;

public class SettingsValidateCommandParameters : GeneratorCommandParameters
{
    public static string[] Names { get; } = [..ToCommand(nameof(SettingsValidateCommand))];

    public SettingsValidateCommandParameters()
        : base(Names.First())
    { }
}
