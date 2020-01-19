using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using KY.Core;
using KY.Generator.Configuration;
using KY.Generator.Exceptions;
using KY.Generator.Extensions;
using KY.Generator.Languages;

namespace KY.Generator.Command
{
    public class CommandReader
    {
        private readonly CommandRegistry commands;
        private readonly List<ILanguage> languages;

        public CommandReader(CommandRegistry commands, List<ILanguage> languages)
        {
            this.commands = commands;
            this.languages = languages;
        }

        public IConfiguration Read(List<string> parameters)
        {
            if (parameters.Count == 0)
            {
                Logger.Error("No command found. Provide at least one command like 'KY.Generator run -configuration=\"<path-to-configuration-file>\"' or 'KY.Generator version'");
                return null;
            }
            List<string> commandList = parameters.Where(x => !x.StartsWith("-")).ToList();
            if (commandList.Count > 1)
            {
                Logger.Error($"Only one command is allowed. All parameters has to start with a dash (-). Commands found: {string.Join(", ", commandList)}");
                return null;
            }
            List<CommandParameter> commandParameters = this.ReadParameters(parameters.Where(x => x.StartsWith("-"))).ToList();

            IConfiguration configuration = this.commands.CreateConfiguration(commandList.Single());
            if (configuration == null)
            {
                Logger.Error($"Command '{commandList.Single()}' not found");
                return null;
            }
            configuration.Environment?.Parameters.AddRange(commandParameters);
            this.SetParameters(configuration, commandParameters);
            if (configuration is IConfigurationWithLanguage configurationWithLanguage)
            {
                configurationWithLanguage.Language = this.languages.FirstOrDefault(language => language.Name.Equals(configurationWithLanguage.LanguageKey, StringComparison.InvariantCultureIgnoreCase))
                                                     ?? configurationWithLanguage.Language;
            }
            return configuration;
        }

        private IEnumerable<CommandParameter> ReadParameters(IEnumerable<string> parameters)
        {
            foreach (string parameter in parameters)
            {
                yield return CommandParameter.Parse(parameter.Trim());
            }
        }

        private void SetParameters(IConfiguration configuration, List<CommandParameter> parameters)
        {
            parameters.AssertIsNotNull(nameof(parameters));
            Type type = configuration.GetType();
            Dictionary<string, PropertyInfo> properties = new Dictionary<string, PropertyInfo>();
            foreach (PropertyInfo property in type.GetProperties())
            {
                if (property.GetCustomAttributes<ConfigurationIgnoreAttribute>().Any())
                {
                    continue;
                }
                properties.Add(property.Name, property);
                property.GetCustomAttributes<ConfigurationPropertyAttribute>().ForEach(attribute => properties.Add(attribute.Alias, property));
            }
            foreach (CommandParameter parameter in parameters)
            {
                string propertyName = parameter.Name.ToPascalCase();
                if (!properties.ContainsKey(propertyName))
                {
                    Logger.Warning($"Unknown parameter '{parameter.Name}'. No matching property '{propertyName}' in {type.Name} found. Add a matching property or use [{nameof(ConfigurationPropertyAttribute).Replace("Attribute", string.Empty)}(\"{propertyName}\")].");
                    continue;
                }
                PropertyInfo propertyInfo = properties[propertyName];
                if (!propertyInfo.CanWrite)
                {
                    throw new ConfigurationParameterReadOnlyException(propertyName);
                }
                object value = CommandParameterConverters.Convert(parameter.Value, propertyInfo);
                propertyInfo.SetValue(configuration, value);
            }
        }
    }
}