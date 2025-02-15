using System.Collections.Generic;
using System.Diagnostics;

namespace KY.Generator.Command;

[DebuggerDisplay("{Name}*{Parameters.Count}")]
public class CliCommand
{
    public string Name { get; }
    public List<CliCommandParameter> Parameters { get; } = new();

    public CliCommand(string name)
    {
        this.Name = name;
    }
}