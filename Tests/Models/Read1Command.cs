using System.Collections.Generic;
using KY.Generator.Command;
using KY.Generator.Configuration;
using KY.Generator.Transfer;

namespace KY.Generator.Tests.Models
{
    internal class Read1Command : IConfigurationCommand
    {
        public bool Execute(IConfiguration configurationBase, List<ITransferObject> transferObjects)
        {
            Read1Configuration configuration = (Read1Configuration)configurationBase;
            return configuration != null;
        }
    }
}