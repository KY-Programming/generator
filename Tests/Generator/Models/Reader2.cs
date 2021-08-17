using System.Collections.Generic;
using KY.Generator.Configuration;
using KY.Generator.Configurations;
using KY.Generator.Transfer;
using KY.Generator.Transfer.Readers;

namespace KY.Generator.Tests.Models
{
    internal class Reader2 : ITransferReader
    {
        public void Read(Read2Configuration configuration, List<ITransferObject> transferObjects)
        {
        }
    }
}
