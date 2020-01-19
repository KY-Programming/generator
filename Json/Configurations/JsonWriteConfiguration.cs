using KY.Generator.Configuration;

namespace KY.Generator.Json.Configurations
{
    public class JsonWriteConfiguration : ConfigurationWithLanguageBase, IFormattableConfiguration
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