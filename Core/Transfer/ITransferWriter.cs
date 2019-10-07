using System.Collections.Generic;
using KY.Generator.Configuration;
using KY.Generator.Output;

namespace KY.Generator.Transfer
{
    public interface ITransferWriter
    {
        void Write(ConfigurationBase configurationBase, List<ITransferObject> transferObjects, IOutput output);
    }
}