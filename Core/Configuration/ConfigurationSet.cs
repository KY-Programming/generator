using System.Collections.Generic;
using KY.Generator.Configurations;

namespace KY.Generator.Configuration
{
    public class ConfigurationSet
    {
        public List<ConfigurationBase> Readers { get; }
        public List<ConfigurationBase> Writers { get; }

        public ConfigurationSet()
        {
            this.Readers = new List<ConfigurationBase>();
            this.Writers = new List<ConfigurationBase>();
        }
    }
}