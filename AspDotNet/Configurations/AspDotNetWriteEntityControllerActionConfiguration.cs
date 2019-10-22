using System.Collections.Generic;

namespace KY.Generator.AspDotNet.Configurations
{
    public class AspDotNetWriteEntityControllerActionConfiguration
    {
        public string Name { get; set; }
        public List<AspDotNetWriteEntityControllerActionAttributeConfiguration> Attributes { get; set; }

        public AspDotNetWriteEntityControllerActionConfiguration()
        {
            this.Attributes = new List<AspDotNetWriteEntityControllerActionAttributeConfiguration>();
        }
    }
}