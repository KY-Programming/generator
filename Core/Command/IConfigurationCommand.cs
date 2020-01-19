using System.Collections.Generic;
using KY.Generator.Configuration;
using KY.Generator.Transfer;

namespace KY.Generator.Command
{
    /// <summary>
    /// Command executed from a configuration file e.g. a generator.json
    /// </summary>
    public interface IConfigurationCommand : ICommand
    {
        bool Execute(IConfiguration configurationBase, List<ITransferObject> transferObjects);
    }
}