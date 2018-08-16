using System;
using System.Collections.Generic;
using System.Linq;
using KY.Generator.Configuration;
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

        public List<ConfigurationBase> Read(string command)
        {
            List<ConfigurationBase> configurations = new List<ConfigurationBase>();

            List<string> chunks = command.Split(' ').Select(x => x.Trim()).ToList();
            CommandConfiguration configuration = new CommandConfiguration(chunks.First());
            foreach (string chunk in chunks.Skip(1))
            {
                configuration.Parameters.Add(chunk[0] == '-' ? CommandValueParameter.Parse(chunk) : new CommandParameter(chunk));
            }
            configuration.Framework = configuration.Parameters.GetValue(nameof(ConfigurationBase.Framework));
            configuration.VerifySsl = configuration.Parameters.GetBoolValue(nameof(ConfigurationBase.VerifySsl), configuration.AddHeader);
            configuration.Language = this.languages.FirstOrDefault(x => x.Name.Equals(configuration.Parameters.GetValue(nameof(ConfigurationBase.Language)), StringComparison.InvariantCultureIgnoreCase))?? configuration.Language;
            configuration.AddHeader = configuration.Parameters.GetBoolValue(nameof(ConfigurationBase.AddHeader), configuration.AddHeader);
            configurations.Add(configuration);
            return configurations;
        }
    }
}