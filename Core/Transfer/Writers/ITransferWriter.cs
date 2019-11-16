using System.Collections.Generic;
using KY.Generator.Configuration;
using KY.Generator.Configurations;
using KY.Generator.Output;

namespace KY.Generator.Transfer.Writers
{
    public interface ITransferWriter
    {
        void Write(ConfigurationBase configurationBase, List<ITransferObject> transferObjects, IOutput output);
    }
}