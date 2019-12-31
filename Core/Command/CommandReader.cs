using System.Collections.Generic;
using System.Linq;
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

        public CommandConfiguration Read(params string[] arguments)
        {
            CommandConfiguration configuration = new CommandConfiguration(arguments.First());
            SetParameters(configuration, arguments.Skip(1));
            configuration.ReadFromParameters(configuration.Parameters, this.languages);
            return configuration;
        }

        public static void SetParameters(CommandConfiguration configuration, IEnumerable<string> parameters)
        {
            foreach (string chunk in parameters)
            {
                string parameter = chunk.Trim();
                configuration.Parameters.Add(parameter[0] == '-' ? CommandValueParameter.Parse(parameter) : new CommandParameter(parameter));
            }
        }
    }
}