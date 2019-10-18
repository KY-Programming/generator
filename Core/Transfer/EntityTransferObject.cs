using System.Collections.Generic;

namespace KY.Generator.Transfer
{
    public class EntityTransferObject : ITransferObject
    {
        public string Name { get; set; }
        public ModelTransferObject Model { get; set; }
        public List<string> Keys { get; set; }
        public string Table { get; set; }
        public string Schema { get; set; }

        public EntityTransferObject()
        {
            this.Keys = new List<string>();
        }
    }
}