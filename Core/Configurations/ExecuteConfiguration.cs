using System.Collections.Generic;
using KY.Generator.Configuration;
using Newtonsoft.Json;

namespace KY.Generator.Configurations
{
    public class ExecuteConfiguration : ConfigurationBase
    {
        public int Version { get; set; }
        public List<string> Load { get; set; }

        [JsonProperty("Output")]
        public string OutputPath { get; set; }


        [ConfigurationProperty("f")]
        public string File { get; set; }

        [ConfigurationProperty("c")]
        [ConfigurationProperty("config")]
        public string Configuration { get; set; }

        [ConfigurationIgnore]
        [JsonIgnore]
        public List<IConfiguration> Execute { get; set; }

        public ExecuteConfiguration()
        {
            this.Execute = new List<IConfiguration>();
        }
    }
}