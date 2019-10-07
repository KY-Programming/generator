using KY.Generator.Configuration;

namespace KY.Generator.Json.Configuration
{
    public class JsonWriteConfiguration : ConfigurationBase
    {
        public JsonWriteReaderConfiguration Reader { get; set; }
        public JsonWriteObjectConfiguration Object { get; set; }
        public bool FormatNames { get; set; }

        public JsonWriteConfiguration()
        {
            this.FormatNames = true;
        }
    }
}