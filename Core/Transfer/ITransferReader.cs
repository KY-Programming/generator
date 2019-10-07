using System.Collections.Generic;
using KY.Generator.Configuration;

namespace KY.Generator.Transfer
{
    public interface ITransferReader
    {
        List<ITransferObject> Read(ConfigurationBase configurationBase);
    }
}