using System.Collections.Generic;
using KY.Generator.Configurations;

namespace KY.Generator.Transfer.Readers
{
    public interface ITransferReader
    {
        void Read(ConfigurationBase configurationBase, List<ITransferObject> transferObjects);
    }
}