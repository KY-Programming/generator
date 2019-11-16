using System.Collections.Generic;
using KY.Generator.Transfer;

namespace KY.Generator.Tsql.Transfers
{
    public class StoredProcedureTransferObject : ITransferObject
    {
        public string Schema { get; set; }
        public string Name { get; set; }
        public TypeTransferObject ReturnType { get; set; }
        public List<TypeTransferObject> Parameters { get; set; }

        public StoredProcedureTransferObject()
        {
            this.Parameters = new List<TypeTransferObject>();
        }
    }
}