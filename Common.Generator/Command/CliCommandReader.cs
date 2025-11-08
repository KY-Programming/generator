using System.Collections.Generic;
using System.Linq;
using KY.Core;

namespace KY.Generator.Command;

public static class CliCommandReader
{
    public static List<CliCommand> Read(params string[] parameters)
    {
        List<CliCommand> commands = [];
        foreach (string parameter in parameters)
        {
            if (parameter.StartsWith("-"))
            {
                if (commands.Count == 0)
                {
                    Logger.Error("The first parameter has to be a command (must not start with a dash '-')");
                    break;
                }
                commands.Last().Parameters.Add(CliCommandParameter.Parse(parameter));
            }
            else
            {
                commands.Add(new CliCommand(parameter));
            }
        }
        return commands;
    }
}