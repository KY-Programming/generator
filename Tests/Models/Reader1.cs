using System.Collections.Generic;
using KY.Generator.Configuration;
using KY.Generator.Transfer;

namespace KY.Generator.Tests.Models
{
    internal class Reader1 : ITransferReader
    {
        public List<ITransferObject> Read(ConfigurationBase configurationBase)
        {
            Read1Configuration configuration = (Read1Configuration)configurationBase;
            return new List<ITransferObject>();
        }
    }
}