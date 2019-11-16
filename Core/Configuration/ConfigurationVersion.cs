using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace KY.Generator.Configuration
{
    internal class ConfigurationVersion
    {
        public int Version { get; set; }
        public List<string> Load { get; set; }
        public ConfigurationFormatting Formatting { get; set; }
        public JContainer Generate { get; set; }
    }
}