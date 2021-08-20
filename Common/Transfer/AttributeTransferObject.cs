using System.Collections.Generic;
using System.Linq;

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

        public AttributeTransferObject(AttributeTransferObject attribute)
        {
            this.Name = attribute.Name;
            this.Namespace = attribute.Namespace;
            this.Value = attribute.Value;
            this.Parameters = attribute.Parameters.ToDictionary(x => x.Key, x => x.Value);
        }

        public AttributeTransferObject Clone()
        {
            return new AttributeTransferObject(this);
        }
    }
}
