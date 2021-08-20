using System.Collections.Generic;
using System.Linq;
using KY.Core;

namespace KY.Generator.Command
{
    public static class RawCommandReader
    {
        public static List<RawCommand> Read(params string[] parameters)
        {
            List<string> globalParameters = new List<string>();
            List<RawCommand> commands = new List<RawCommand>();
            foreach (string parameter in parameters)
            {
                if (parameter.StartsWith("-*"))
                {
                    globalParameters.Add(parameter);
                }
                else if (parameter.StartsWith("-"))
                {
                    if (commands.Count == 0)
                    {
                        Logger.Error("The first parameter has to be a command (must not start with a dash '-')");
                        break;
                    }
                    commands.Last().Parameters.Add(RawCommandParameter.Parse(parameter));
                }
                else
                {
                    commands.Add(new RawCommand(parameter));
                }
            }
            List<RawCommandParameter> parsedGlobalParameters = globalParameters.Select(RawCommandParameter.Parse).ToList();
            foreach (RawCommand command in commands)
            {
                command.Parameters.AddRange(parsedGlobalParameters);
            }
            return commands;
        }
    }
}
