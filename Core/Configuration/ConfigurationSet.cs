using System.Collections.Generic;
using KY.Generator.Configurations;

namespace KY.Generator.Configuration
{
    public class ConfigurationSet
    {
        public List<ConfigurationBase> Configurations { get; }

        public ConfigurationSet()
        {
            this.Configurations = new List<ConfigurationBase>();
        }
    }
}