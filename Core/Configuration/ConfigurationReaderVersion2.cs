using System;
using System.Collections.Generic;
using System.Linq;
using KY.Core;
using KY.Core.Dependency;
using KY.Generator.Command;
using KY.Generator.Configurations;
using KY.Generator.Exceptions;
using KY.Generator.Extensions;
using KY.Generator.Languages;
using KY.Generator.Load;
using Newtonsoft.Json.Linq;

namespace KY.Generator.Configuration
{
    internal class ConfigurationReaderVersion2 : IConfigurationReaderVersion
    {
        private readonly IDependencyResolver resolver;

        public int Version => 2;

        public ConfigurationReaderVersion2(IDependencyResolver resolver)
        {
            this.resolver = resolver;
        }

        public ExecuteConfiguration Read(JObject rawConfiguration)
        {
            ExecuteConfiguration configuration = rawConfiguration.ToObject<ExecuteConfiguration>();
            if (configuration.Load != null && configuration.Load.Count > 0)
            {
                this.resolver.Create<GeneratorModuleLoader>().Load(configuration.Load);
            }
            ConfigurationVersion2 configuration2 = rawConfiguration.ToObject<ConfigurationVersion2>();
            if (configuration2.Execute == null)
            {
                throw new InvalidConfigurationException("Invalid configuration found. 'execute' tag is missing.");
            }
            if (configuration2.Execute is JObject obj)
            {
                configuration.Execute.Add(this.ReadConfiguration(obj));
            }
            else if (configuration2.Execute is JArray array)
            {
                foreach (JToken token in array)
                {
                    if (token is JObject jObject)
                    {
                        configuration.Execute.Add(this.ReadConfiguration(jObject));
                    }
                    else
                    {
                        throw new InvalidConfigurationException($"Invalid configuration found. Unknown entry in 'execute' tag found. Expected 'Object' but got '{token.Type}'");
                    }
                }
            }
            return configuration;
        }

        private IConfiguration ReadConfiguration(JObject rawConfiguration)
        {
            CommandRegistry commands = this.resolver.Get<CommandRegistry>();
            List<string> groups = commands.GetGroups();
            JProperty property = rawConfiguration.Properties().FirstOrDefault(p => groups.Any(action => action.Equals(p.Name, StringComparison.InvariantCultureIgnoreCase)));
            if (property == null)
            {
                JProperty firstProperty = rawConfiguration.Properties().FirstOrDefault();
                string command = firstProperty == null ? "{ <empty-object> }" : $"{{ \"{firstProperty.Name}\": \"{firstProperty.Value}\", ... }}";
                throw new UnknownCommandException(command);
            }
            Type type = commands.GetConfigurationType(property.Value.ToString(), property.Name);
            IConfiguration configuration = (IConfiguration)rawConfiguration.ToObject(type);
            configuration.Environment.Command = property.Name;
            configuration.Environment.CommandGroup = property.Value.ToString();
            if (configuration is IConfigurationWithLanguage configurationWithLanguage)
            {
                List<ILanguage> languages = this.resolver.Get<List<ILanguage>>();
                configurationWithLanguage.Language = languages.FirstOrDefault(language => language.Name.Equals(configurationWithLanguage.LanguageKey, StringComparison.InvariantCultureIgnoreCase))
                                                         ?? configurationWithLanguage.Language;
            }
            return configuration;
        }
    }
}