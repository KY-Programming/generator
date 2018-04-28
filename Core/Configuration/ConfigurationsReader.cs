using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using KY.Core;

namespace KY.Generator.Configuration
{
    internal class ConfigurationsReader
    {
        private readonly Dictionary<string, IConfigurationReader> readers;
        private readonly List<string> ignoredNames;

        public ConfigurationsReader(IEnumerable<IConfigurationReader> readers)
        {
            this.readers = readers.ToDictionary(x => x.TagName, x => x);
            this.ignoredNames = new List<string> { "VerifySSL", "Language", "AddHeader" };
        }

        public IEnumerable<ConfigurationBase> Read(XElement rootElement)
        {
            foreach (XElement configurationElement in rootElement.Elements())
            {
                string name = configurationElement.Name.LocalName;
                if (this.ignoredNames.Contains(name))
                {
                    continue;
                }
                if (!this.readers.ContainsKey(name))
                {
                    Logger.Trace($"Skipped element with name {name}: No generator found");
                    continue;
                }
                yield return this.readers[name].Read(rootElement, configurationElement);
            }
        }
    }
}