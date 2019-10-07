using System.Collections.Generic;

namespace KY.Generator.Configuration
{
    public class ConfigurationPair
    {
        public List<ConfigurationBase> Readers { get; }
        public List<ConfigurationBase> Writers { get; }

        public ConfigurationPair()
        {
            this.Readers = new List<ConfigurationBase>();
            this.Writers = new List<ConfigurationBase>();
        }
    }
}