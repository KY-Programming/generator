using KY.Core;
using KY.Generator.Command;

namespace KY.Generator;

public static class GeneratorErrors
{
    public static void CommandDocumentationHint()
    {
        Logger.Error("See our Documentation: https://generator.ky-programming.de/");
    }

    public static void CommandNotFoundError(CliCommand command)
    {
        CommandNotFoundError(command.Name);
    }

    public static void CommandNotFoundError(string command)
    {
        Logger.Error($"Command '{command}' not found");
    }
}
