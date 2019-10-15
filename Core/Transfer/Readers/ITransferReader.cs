using System.Collections.Generic;
using KY.Generator.Configuration;

namespace KY.Generator.Transfer.Readers
{
    public interface ITransferReader
    {
        void Read(ConfigurationBase configurationBase, List<ITransferObject> transferObjects);
    }
}