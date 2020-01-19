using System.Collections.Generic;
using KY.Generator.Command;
using KY.Generator.Configuration;
using KY.Generator.Transfer;

namespace KY.Generator.Tests.Models
{
    internal class Write2Command : IConfigurationCommand
    {
        public bool Execute(IConfiguration configurationBase, List<ITransferObject> transferObjects)
        {
            Write2Configuration configuration = (Write2Configuration)configurationBase;
            return configuration != null;
        }
    }
}