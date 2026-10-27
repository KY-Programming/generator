using System.Collections.Generic;

namespace KY.Generator.Transfer
{
    public class HttpServiceActionTransferObject
    {
        public string Name { get; set; }
        public TypeTransferObject ReturnType { get; set; }
        public string Route { get; set; }
        public bool RequireBodyParameter { get; set; }
        public bool CanHaveBodyParameter { get; set; }
        public List<HttpServiceActionParameterTransferObject> Parameters { get; }
        public HttpServiceActionTypeTransferObject Type { get; set; }
        public string Version { get; set; }
        public bool FixCasingWithMapping { get; set; }

        /// <summary>
        /// The action answers with the raw content of a file instead of a serialized value. The return type is void in
        /// that case, the client has to read the body as binary and the file name from the Content-Disposition header
        /// </summary>
        public bool ReturnsFile { get; set; }

        public HttpServiceActionTransferObject()
        {
            this.Parameters = new List<HttpServiceActionParameterTransferObject>();
        }
    }
}
