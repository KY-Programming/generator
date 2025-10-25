using KY.Generator.Command;

namespace KY.Generator.Commands;

public class LoadCommandParameters : GeneratorCommandParameters
{
    [GeneratorParameter("assembly")]
    public string Assembly { get; set; }
}
