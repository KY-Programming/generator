using System.Collections.Generic;
using KY.Generator.Templates;
using KY.Generator.Transfer;

namespace KY.Generator.Models
{
    public class GeneratorEnvironment
    {
        public List<ITransferObject> TransferObjects { get; }

        public GeneratorEnvironment()
        {
            this.TransferObjects = new List<ITransferObject>();
        }
    }
}