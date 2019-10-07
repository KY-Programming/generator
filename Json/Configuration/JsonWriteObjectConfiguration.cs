namespace KY.Generator.Json.Configuration
{
    public class JsonWriteObjectConfiguration
    {
        public string Name { get; set; }
        public string Namespace { get; set; }
        public string RelativePath { get; set; }
        public bool SkipNamespace { get; set; }
        public bool FieldsToProperties { get; set; }    
        public bool PropertiesToFields { get; set; }
        public bool FormatNames { get; set; }
        public bool WithReader { get; set; }
    }
}