using KY.Generator.Configurations;
using Newtonsoft.Json.Linq;

namespace KY.Generator.Configuration
{
    internal interface IConfigurationReaderVersion
    {
        int Version { get; }
        ExecuteConfiguration Read(JObject rawConfiguration);
    }
}