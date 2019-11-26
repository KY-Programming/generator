using System.Collections.Generic;

namespace KY.Generator.Transfer
{
    public class EntityTransferObject : ITransferObject
    {
        public string Name { get; set; }
        public string Table { get; set; }
        public string Schema { get; set; }
        public ModelTransferObject Model { get; set; }
        public List<EntityKeyTransferObject> Keys { get; set; }
        public List<EntityActionTransferObject> Actions { get; set; }

        public EntityTransferObject()
        {
            this.Keys = new List<EntityKeyTransferObject>();
            this.Actions = new List<EntityActionTransferObject>();
        }
    }
}