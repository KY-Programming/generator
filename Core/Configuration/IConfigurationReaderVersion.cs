using System.Collections.Generic;

namespace KY.Generator.Configuration
{
    internal interface IConfigurationReaderVersion
    {
        int Version { get; }
        List<ConfigurationSet> Read(ConfigurationVersion version);
    }
}