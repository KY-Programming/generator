using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace KY.Generator.Configuration
{
    internal interface IConfigurationReaderVersion
    {
        int Version { get; }
        List<ConfigurationPair> Read(JToken token);
    }
}