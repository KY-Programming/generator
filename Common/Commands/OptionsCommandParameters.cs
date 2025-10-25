using KY.Generator.Command;

namespace KY.Generator.Commands;

public class OptionsCommandParameters : GeneratorCommandParameters
{
    public string? Statistics { get; set; }
    public string? Output { get; set; }
}
