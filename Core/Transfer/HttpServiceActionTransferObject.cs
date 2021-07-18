using System.Collections.Generic;

namespace KY.Generator.Transfer
{
    public class HttpServiceActionTransferObject
    {
        public string Name { get; set; }
        public TypeTransferObject ReturnType { get; set; }
        public string Route { get; set; }
        public bool RequireBodyParameter { get; set; }
        public List<HttpServiceActionParameterTransferObject> Parameters { get; }
        public HttpServiceActionTypeTransferObject Type { get; set; }
        public string Version { get; set; }

        public HttpServiceActionTransferObject()
        {
            this.Parameters = new List<HttpServiceActionParameterTransferObject>();
        }
    }
}
