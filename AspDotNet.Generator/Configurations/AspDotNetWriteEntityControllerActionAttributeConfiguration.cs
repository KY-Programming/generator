using System.Collections.Generic;

namespace KY.Generator.AspDotNet.Configurations
{
    public class AspDotNetWriteEntityControllerActionAttributeConfiguration
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public List<AspDotNetWriteEntityControllerActionAttributeValueConfiguration> Values { get; set; }

        public AspDotNetWriteEntityControllerActionAttributeConfiguration()
        {
            this.Values = new List<AspDotNetWriteEntityControllerActionAttributeValueConfiguration>();
        }
    }
}