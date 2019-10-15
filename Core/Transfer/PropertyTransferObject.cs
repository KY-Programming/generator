using System.Collections.Generic;

namespace KY.Generator.Transfer
{
    public class PropertyTransferObject
    {
        public string Name { get; set; }
        public TypeTransferObject Type { get; set; }
        public bool CanRead { get; set; } = true;
        public bool CanWrite { get; set; } = true;
        public List<AttributeTransferObject> Attributes { get; }
        public string Comment { get; set; }

        public PropertyTransferObject()
        {
            this.Attributes = new List<AttributeTransferObject>();
        }
    }
}