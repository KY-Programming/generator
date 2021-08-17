using System.Collections.Generic;
using KY.Generator.Configuration;
using KY.Generator.Configurations;
using KY.Generator.Transfer;
using KY.Generator.Transfer.Readers;

namespace KY.Generator.Tests.Models
{
    internal class Reader1 : ITransferReader
    {
        public void Read(Read1Configuration configuration, List<ITransferObject> transferObjects)
        {
        }
    }
}
