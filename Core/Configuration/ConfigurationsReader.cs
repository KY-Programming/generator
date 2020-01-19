using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using KY.Core;
using KY.Generator.Configurations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KY.Generator.Configuration
{
    internal class ConfigurationsReader
    {
        private readonly Dictionary<int, IConfigurationReaderVersion> readers;

        public ConfigurationsReader(IEnumerable<IConfigurationReaderVersion> readers)
        {
            this.readers = readers.ToDictionary(x => x.Version, x => x);
        }

        public ExecuteConfiguration Parse(string json)
        {
            using (JsonTextReader reader = new JsonTextReader(new StringReader(json)))
            {
                JObject rawConfiguration;
                try
                {
                    rawConfiguration = JObject.Load(reader);
                }
                catch (Exception exception)
                {
                    throw new InvalidConfigurationException("Invalid json. See inner exception for details", exception);
                }
                ConfigurationVersion configuration = rawConfiguration.ToObject<ConfigurationVersion>();
                if (configuration.Version == 0)
                {
                    configuration.Version = this.readers.Max(x => x.Key);
                    Logger.Warning($"No version found. Fallback to {configuration.Version}");
                }
                if (this.readers.ContainsKey(configuration.Version))
                {
                    return this.readers[configuration.Version].Read(rawConfiguration);
                }
                throw new InvalidConfigurationException($"No reader for version {configuration.Version} found");
            }
        }
    }
}