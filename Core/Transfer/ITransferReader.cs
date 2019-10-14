using System.Collections.Generic;
using KY.Generator.Configuration;

namespace KY.Generator.Transfer
{
    public interface ITransferReader
    {
        void Read(ConfigurationBase configurationBase, List<ITransferObject> transferObjects);
    }
}