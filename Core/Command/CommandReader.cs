using System.Collections.Generic;
using System.Linq;
using KY.Core;
using KY.Generator.Command.Extensions;
using KY.Generator.Languages;

namespace KY.Generator.Command
{
    internal class CommandReader
    {
        private readonly List<ILanguage> languages;

        public CommandReader(List<ILanguage> languages)
        {
            this.languages = languages;
        }

        public CommandConfiguration Read(params string[] parameters)
        {
            if (parameters.Length == 0)
            {
                Logger.Error("No command found. Provide at least one command like 'run -configuration=\"<path-to-configuration-file>\"'");
                return null;
            }
            List<string> commandList = parameters.Where(x => !x.StartsWith("-")).ToList();
            if (commandList.Count > 1)
            {
                Logger.Error($"Only one command is allowed. All parameters has to start with a dash (-). Commands found: {string.Join(", ", commandList)}");
                return null;
            }
            List<CommandParameter> commandParameters = this.ReadParameters(parameters.Where(x => x.StartsWith("-"))).ToList();
            CommandConfiguration configuration = new CommandConfiguration(commandList.Single(), commandParameters);
            configuration.ReadFromParameters(configuration.Parameters, this.languages);
            return configuration;
        }

        private IEnumerable<CommandParameter> ReadParameters(IEnumerable<string> parameters)
        {
            foreach (string parameter in parameters)
            {
                yield return CommandParameter.Parse(parameter.Trim());
            }
        }
    }
}