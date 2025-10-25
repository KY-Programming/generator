using KY.Generator.Command;

namespace KY.Generator.Commands;

public class ReadProjectCommandParameters : GeneratorCommandParameters
{
    public string Solution { get; set; }
    public string Project { get; set; }
}
