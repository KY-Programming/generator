using KY.Generator.Angular.Commands;

namespace KY.Generator.Angular.Configurations
{
    public class AngularWriteHttpClientConfiguration
    {
        public string Name { get; set; }
        public string Import { get; set; }
        public string Get { get; set; }
        public AngularHttpClientMethodOptions HasGetOptions { get; set; }
        public string Post { get; set; }
        public AngularHttpClientMethodOptions HasPostOptions { get; set; }
        public string Patch { get; set; }
        public AngularHttpClientMethodOptions HasPatchOptions { get; set; }
        public string Put { get; set; }
        public AngularHttpClientMethodOptions HasPutOptions { get; set; }
        public string Delete { get; set; }
        public AngularHttpClientMethodOptions HasDeleteOptions { get; set; }
    }
}
