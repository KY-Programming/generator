using System.Collections.Generic;
using KY.Generator.Configuration;
using KY.Generator.Output;
using KY.Generator.Transfer;
using KY.Generator.Transfer.Writers;

namespace KY.Generator.Tests.Models
{
    internal class Writer2 : ITransferWriter
    {
        public void Write(ConfigurationBase configurationBase, List<ITransferObject> transferObjects, IOutput output)
        {
            Write2Configuration configuration = (Write2Configuration)configurationBase;
        }
    }
}