using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using KY.Core;
using KY.Core.Dependency;
using KY.Generator.Load;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KY.Generator.Configuration
{
    internal class ConfigurationsReader
    {
        private readonly IDependencyResolver resolver;
        private readonly Dictionary<int, IConfigurationReaderVersion> versions;

        public ConfigurationsReader(IDependencyResolver resolver, IEnumerable<IConfigurationReaderVersion> versions)
        {
            this.resolver = resolver;
            this.versions = versions.ToDictionary(x => x.Version, x => x);
        }

        //public List<ConfigurationBase> Read()
        //{
        //    return null;
        //}

        public List<ConfigurationPair> Parse(string json)
        {
            using (JsonTextReader reader = new JsonTextReader(new StringReader(json)))
            {
                JObject jObject;
                try
                {
                    jObject = JObject.Load(reader);
                }
                catch (Exception exception)
                {
                    throw new InvalidConfigurationException("Can not load json. See inner exception for details", exception);
                }
                ConfigurationVersion version = jObject.ToObject<ConfigurationVersion>();
                if (version?.Generate == null)
                {
                    throw new InvalidConfigurationException("Unsupported configuration found. Generate tag is missing.");
                }
                if (version.Version == 0)
                {
                    version.Version = this.versions.Max(x => x.Key);
                    Logger.Warning($"No version found. Fallback to {version.Version}");
                }
                if (version.Load != null && version.Load.Count > 0)
                {
                    this.resolver.Create<GeneratorModuleLoader>().Load(version.Load);
                }
                if (this.versions.ContainsKey(version.Version))
                {
                    return this.versions[version.Version].Read(version);
                }
                throw new InvalidConfigurationException($"No reader for version {version.Version} found");
            }
        }
    }
}