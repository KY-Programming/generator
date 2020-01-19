using System.Collections.Generic;
using KY.Generator.Command;
using KY.Generator.Configuration;
using KY.Generator.Transfer;

namespace KY.Generator.Tests.Models
{
    internal class Read2Command : IConfigurationCommand
    {
        public bool Execute(IConfiguration configurationBase, List<ITransferObject> transferObjects)
        {
            Read2Configuration configuration = (Read2Configuration)configurationBase;
            return configuration != null;
        }
    }
}