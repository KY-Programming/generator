using System.Collections.Generic;

namespace KY.Generator.Transfer
{
    public class AttributeTransferObject
    {
        public string Name { get; set; }
        public string Namespace { get; set; }
        public object Value { get; set; }
        public Dictionary<string, object> Parameters { get; }

        public AttributeTransferObject()
        {
            this.Parameters = new Dictionary<string, object>();
        }
    }
}