using System.Collections.Generic;
using System.Linq;
using KY.Core;

namespace KY.Generator.Command;

public static class CliCommandReader
{
    private const string GlobalParameterPrefix = "-*";

    /// <summary>
    /// Reads the commands and the parameters that belong to each of them. Global parameters are left out - they
    /// belong to every command, see <see cref="ReadGlobal"/>
    /// </summary>
    public static List<CliCommand> Read(params string[] parameters)
    {
        List<CliCommand> commands = [];
        foreach (string parameter in parameters.Where(x => !IsGlobal(x)))
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

    /// <summary>
    /// Reads the global parameters (<c>-*name</c> or <c>-*name=value</c>). They apply to every command, so it does
    /// not matter where in the command line they stand
    /// </summary>
    public static List<CliCommandParameter> ReadGlobal(params string[] parameters)
    {
        return parameters.Where(IsGlobal).Select(CliCommandParameter.Parse).ToList();
    }

    public static bool IsGlobal(string parameter)
    {
        return parameter.StartsWith(GlobalParameterPrefix);
    }
}
