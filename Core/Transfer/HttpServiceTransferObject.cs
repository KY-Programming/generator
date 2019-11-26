using System.Collections.Generic;
using KY.Generator.Languages;

namespace KY.Generator.Transfer
{
    public class HttpServiceTransferObject : ITransferObject
    {
        public string Name { get; set; }
        public string Route { get; set; }
        public ILanguage Language { get; set; }

        public List<HttpServiceActionTransferObject> Actions { get; }

        public HttpServiceTransferObject()
        {
            this.Actions = new List<HttpServiceActionTransferObject>();
        }
    }
}