namespace KY.Generator.Json.Configurations
{
    public class JsonWriteConfiguration
    {
        public string Name { get; set; }
        public string Namespace { get; set; }
        public string RelativePath { get; set; }
        public bool WithReader { get; set; }

        public JsonWriteConfiguration()
        {
            // this.FormatNames = true;
        }
    }
}
