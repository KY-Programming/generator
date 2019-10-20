using System.Collections.Generic;

namespace KY.Generator.OData.Configuration
{
    public class ODataWriteControllerConfiguration
    {
        public string RelativePath { get; set; }
        public string Namespace { get; set; }
        public string Route { get; set; }
        public string BasedOn { get; set; }
        public ODataWriteControllerMethodConfiguration Get { get; set; }
        public ODataWriteControllerMethodConfiguration GetSingle { get; set; }
        public ODataWriteControllerMethodConfiguration Post { get; set; }
        public ODataWriteControllerMethodConfiguration Delete { get; set; }
        public ODataWriteControllerMethodConfiguration Put { get; set; }
        public ODataWriteControllerMethodConfiguration Patch { get; set; }
        public List<string> Usings { get; }

        public ODataWriteControllerConfiguration()
        {
            this.Usings = new List<string>();
        }
    }
}