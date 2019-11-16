using KY.Generator.Configuration;
using KY.Generator.Configurations;

namespace KY.Generator.Json.Configuration
{
    public class JsonWriteConfiguration : ConfigurationBase, IFormattableConfiguration
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