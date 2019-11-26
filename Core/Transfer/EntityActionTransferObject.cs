using System.Collections.Generic;

namespace KY.Generator.Transfer
{
    public class EntityActionTransferObject
    {
        public string Name { get; set; }
        public string Namespace { get; set; }
        public TypeTransferObject ReturnType { get; set; }
        public List<EntityActionParameterTransferObject> Parameters { get; set; }

        public EntityActionTransferObject()
        {
            this.ReturnType = new TypeTransferObject { Name = "void" };
            this.Parameters = new List<EntityActionParameterTransferObject>();
        }
    }
}