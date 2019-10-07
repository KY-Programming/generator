using System.Collections.Generic;
using KY.Generator.Configuration;
using KY.Generator.Transfer;

namespace KY.Generator.Tests.Models
{
    internal class Reader2 : ITransferReader
    {
        public List<ITransferObject> Read(ConfigurationBase configurationBase)
        {
            Read2Configuration configuration = (Read2Configuration)configurationBase;
            return new List<ITransferObject>();
        }
    }
}