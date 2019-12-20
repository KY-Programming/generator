using System;
using System.Collections.Generic;
using System.Linq;
using KY.Core.Dependency;
using KY.Generator.Configurations;
using KY.Generator.Languages;
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

        public List<ConfigurationSet> Read(ConfigurationVersion version)
        {
            List<ConfigurationSet> list = new List<ConfigurationSet>();
            if (version.Generate is JObject obj)
            {
                ConfigurationSet set = new ConfigurationSet();
                this.ReadToSet(obj, set);
                list.Add(set);
            }
            else if (version.Generate is JArray array)
            {
                if (array.First is JObject)
                {
                    ConfigurationSet set = new ConfigurationSet();
                    foreach (JObject entry in array.OfType<JObject>())
                    {
                        this.ReadToSet(entry, set);
                    }
                    list.Add(set);
                }
                else if (array.First is JArray)
                {
                    foreach (JArray innerArray in array.OfType<JArray>())
                    {
                        ConfigurationSet set = new ConfigurationSet();
                        foreach (JObject entry in innerArray.OfType<JObject>())
                        {
                            this.ReadToSet(entry, set);
                        }
                        list.Add(set);
                    }
                }
            }
            list.ForEach(pair => pair.Configurations.ForEach(configuration => configuration.Formatting = configuration.Formatting ?? version.Formatting));
            return list;
        }

        private void ReadToSet(JObject token, ConfigurationSet set)
        {
            ConfigurationMapping mapping = this.resolver.Get<ConfigurationMapping>();
            List<string> actions = mapping.GetActions();
            JProperty property = token.Properties().FirstOrDefault(p => actions.Any(action => action.Equals(p.Name, StringComparison.InvariantCultureIgnoreCase)));
            string configurationName = property?.Value?.ToString();
            if (configurationName != null)
            {
                ConfigurationBase configurationBase = (ConfigurationBase)token.ToObject(mapping.ResolveConfiguration(configurationName, property.Name));
                this.Prepare(configurationBase);
                set.Configurations.Add(configurationBase);
            }
        }

        private void Prepare(ConfigurationBase configurationBase)
        {
            configurationBase.Language = configurationBase.Language ?? this.resolver.Get<List<ILanguage>>().FirstOrDefault(x => x.Name.Equals(configurationBase.LanguageKey, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}