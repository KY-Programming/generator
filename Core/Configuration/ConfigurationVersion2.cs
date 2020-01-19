using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KY.Generator.Configuration
{
    internal class ConfigurationVersion2 : ConfigurationVersion
    {
        [JsonProperty("generate")]
        public JToken Execute { get; set; }
    }
}