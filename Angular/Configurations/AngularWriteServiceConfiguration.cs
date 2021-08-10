using System.Collections.Generic;

namespace KY.Generator.Angular.Configurations
{
    public class AngularWriteServiceConfiguration
    {
        public string Name { get; set; }
        public string RelativePath { get; set; }
        public AngularWriteHttpClientConfiguration HttpClient { get; set; } = new();
        public bool EndlessTries { get; set; }
        public List<int> Timeouts { get; set; }
    }
}
