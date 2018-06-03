using KY.Generator.Configuration;

namespace KY.Generator.Json.Configuration
{
    public class JsonConfiguration : ConfigurationBase
    {
        public JsonToObjectConfiguration ToObject { get; set; }
        public JsonToObjectReaderConfiguration WithReader { get; set; }
    }
}