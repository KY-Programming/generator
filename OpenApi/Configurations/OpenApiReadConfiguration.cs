using KY.Generator.Configurations;

namespace KY.Generator.OpenApi.Configurations
{
    public class OpenApiReadConfiguration : ConfigurationBase
    {
        public string File { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public string Namespace { get; set; }
        public bool DataAnnotations { get; set; }
    }
}