using System.Collections.Generic;

namespace KY.Generator.Transfer
{
    public class MethodTransferObject
    {
        public string Name { get; set; }
        public string Comment { get; set; }
        public TypeTransferObject ReturnType { get; set; }
        public List<MethodParameterTransferObject> Parameters { get; }

        public MethodTransferObject()
        {
            this.Parameters = new List<MethodParameterTransferObject>();
        }
    }
}