using System.Collections.Generic;

namespace KY.Generator.AspDotNet.Configurations
{
    internal class AspDotNetWriteEntityControllerConfiguration
    {
        public string Entity { get; set; }
        public string Name { get; set; }
        public string Namespace { get; set; }
        public string Route { get; set; }
        public List<string> Usings { get; set; }
        public AspDotNetWriteEntityControllerActionConfiguration Get { get; set; }
        public AspDotNetWriteEntityControllerActionConfiguration Post { get; set; }
        public AspDotNetWriteEntityControllerActionConfiguration Patch { get; set; }
        public AspDotNetWriteEntityControllerActionConfiguration Put { get; set; }
        public AspDotNetWriteEntityControllerActionConfiguration Delete { get; set; }

        public AspDotNetWriteEntityControllerConfiguration()
        {
            this.Usings = new List<string>();
        }
    }
}