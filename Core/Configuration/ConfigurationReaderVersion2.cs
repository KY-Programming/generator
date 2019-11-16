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

        public List<ConfigurationPair> Read(ConfigurationVersion version)
        {
            List<ConfigurationPair> list = new List<ConfigurationPair>();
            if (version.Generate is JObject obj)
            {
                ConfigurationPair pair = new ConfigurationPair();
                this.ReadToPair(obj, pair);
                list.Add(pair);
            }
            else if (version.Generate is JArray array)
            {
                ConfigurationPair pair = new ConfigurationPair();
                foreach (JToken entry in array)
                {
                    this.ReadToPair(entry, pair);
                }
                list.Add(pair);
            }
            list.ForEach(pair => pair.Readers.ForEach(reader => reader.Formatting = reader.Formatting ?? version.Formatting));
            list.ForEach(pair => pair.Writers.ForEach(writer => writer.Formatting = writer.Formatting ?? version.Formatting));
            return list;
        }

        private void ReadToPair(JToken token, ConfigurationPair pair)
        {
            ReadOrWriteConfiguration configuration = token.ToObject<ReadOrWriteConfiguration>();
            ReaderConfigurationMapping readers = this.resolver.Get<ReaderConfigurationMapping>();
            WriterConfigurationMapping writers = this.resolver.Get<WriterConfigurationMapping>();
            if (configuration.Read != null)
            {
                ConfigurationBase configurationBase = token.ToObject(readers.Resolve(configuration.Read)) as ConfigurationBase;
                this.Prepare(configurationBase);
                pair.Readers.Add(configurationBase);
            }
            if (configuration.Write != null)
            {
                ConfigurationBase configurationBase = token.ToObject(writers.Resolve(configuration.Write)) as ConfigurationBase;
                this.Prepare(configurationBase);
                pair.Writers.Add(configurationBase);
            }
        }

        private void Prepare(ConfigurationBase configurationBase)
        {
            configurationBase.Language = configurationBase.Language ?? this.resolver.Get<List<ILanguage>>().FirstOrDefault(x => x.Name.Equals(configurationBase.LanguageKey, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}