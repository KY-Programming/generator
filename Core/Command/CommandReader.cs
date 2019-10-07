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
            foreach (string chunk in arguments.Skip(1))
            {
                configuration.Parameters.Add(chunk[0] == '-' ? CommandValueParameter.Parse(chunk) : new CommandParameter(chunk));
            }
            configuration.ReadFromParameters(configuration.Parameters, this.languages);
            return configuration;
        }
    }
}