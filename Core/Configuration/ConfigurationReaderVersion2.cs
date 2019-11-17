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
                this.ReadToPair(obj, set);
                list.Add(set);
            }
            else if (version.Generate is JArray array)
            {
                if (array.First is JObject)
                {
                    ConfigurationSet set = new ConfigurationSet();
                    foreach (JObject entry in array.OfType<JObject>())
                    {
                        this.ReadToPair(entry, set);
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
                            this.ReadToPair(entry, set);
                        }
                        list.Add(set);
                    }
                }
            }
            list.ForEach(pair => pair.Readers.ForEach(reader => reader.Formatting = reader.Formatting ?? version.Formatting));
            list.ForEach(pair => pair.Writers.ForEach(writer => writer.Formatting = writer.Formatting ?? version.Formatting));
            return list;
        }

        private void ReadToPair(JToken token, ConfigurationSet set)
        {
            ReadOrWriteConfiguration configuration = token.ToObject<ReadOrWriteConfiguration>();
            ReaderConfigurationMapping readers = this.resolver.Get<ReaderConfigurationMapping>();
            WriterConfigurationMapping writers = this.resolver.Get<WriterConfigurationMapping>();
            if (configuration.Read != null)
            {
                ConfigurationBase configurationBase = token.ToObject(readers.Resolve(configuration.Read)) as ConfigurationBase;
                this.Prepare(configurationBase);
                set.Readers.Add(configurationBase);
            }
            if (configuration.Write != null)
            {
                ConfigurationBase configurationBase = token.ToObject(writers.Resolve(configuration.Write)) as ConfigurationBase;
                this.Prepare(configurationBase);
                set.Writers.Add(configurationBase);
            }
        }

        private void Prepare(ConfigurationBase configurationBase)
        {
            configurationBase.Language = configurationBase.Language ?? this.resolver.Get<List<ILanguage>>().FirstOrDefault(x => x.Name.Equals(configurationBase.LanguageKey, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}