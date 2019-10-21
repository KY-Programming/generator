using System.Collections.Generic;

namespace KY.Generator.Transfer
{
    public class EntityTransferObject : ITransferObject
    {
        public string Name { get; set; }
        public ModelTransferObject Model { get; set; }
        public List<EntityKeyTransferObject> Keys { get; set; }
        public string Table { get; set; }
        public string Schema { get; set; }

        public EntityTransferObject()
        {
            this.Keys = new List<EntityKeyTransferObject>();
        }
    }

    public class EntityKeyTransferObject
    {
        public string Name { get; set; }
        public TypeTransferObject Type { get; set; }
        public PropertyTransferObject Property { get; set; }
    }
}