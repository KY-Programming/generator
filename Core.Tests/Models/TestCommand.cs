using System.Collections.Generic;
using KY.Generator.Command;
using KY.Generator.Configuration;
using KY.Generator.Transfer;

namespace KY.Generator.Core.Tests.Models
{
    internal class TestCommand : IConfigurationCommand
    {
        public bool Execute(IConfiguration configuration, List<ITransferObject> transferObjects)
        {
            return true;
        }
    }
}