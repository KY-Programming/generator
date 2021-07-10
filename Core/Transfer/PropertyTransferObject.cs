using System.Collections.Generic;
using System.Linq;

namespace KY.Generator.Transfer
{
    public class PropertyTransferObject
    {
        public string Name { get; set; }
        public TypeTransferObject Type { get; set; }
        public bool CanRead { get; set; } = true;
        public bool CanWrite { get; set; } = true;
        public List<AttributeTransferObject> Attributes { get; set; } = new();
        public string Comment { get; set; }

        public PropertyTransferObject()
        { }

        public PropertyTransferObject(PropertyTransferObject property)
        {
            this.Name = property.Name;
            this.Type = property.Type.Clone();
            this.CanRead = property.CanRead;
            this.CanRead = property.CanWrite;
            this.Attributes = property.Attributes.Select(attribute => attribute.Clone()).ToList();
            this.Comment = property.Comment;
        }
    }
}
