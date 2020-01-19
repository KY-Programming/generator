using KY.Generator.Configuration;

namespace KY.Generator.Configurations
{
    public class CookieConfiguration : ConfigurationBase
    {
        public string Url { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Path { get; set; }
        public string Domain { get; set; }
    }
}