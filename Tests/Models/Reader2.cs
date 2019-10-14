using System.Collections.Generic;
using KY.Generator.Configuration;
using KY.Generator.Transfer;

namespace KY.Generator.Tests.Models
{
    internal class Reader2 : ITransferReader
    {
        public void Read(ConfigurationBase configurationBase, List<ITransferObject> transferObjects)
        {
            Read2Configuration configuration = (Read2Configuration)configurationBase;
        }
    }
}