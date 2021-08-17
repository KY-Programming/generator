using System.Collections.Generic;
using KY.Generator.Languages;
using KY.Generator.Models;

namespace KY.Generator.Transfer
{
    public class SignalRHubTransferObject : ITransferObject
    {
        public string Name { get; set; }
        public ILanguage Language { get; set; }

        public List<HttpServiceActionTransferObject> Actions { get; }
        public List<HttpServiceActionTransferObject> Events { get; }

        public SignalRHubTransferObject()
        {
            this.Actions = new List<HttpServiceActionTransferObject>();
            this.Events = new List<HttpServiceActionTransferObject>();
        }
    }
}
