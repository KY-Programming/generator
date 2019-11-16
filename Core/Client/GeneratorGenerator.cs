using System.Collections.Generic;
using KY.Core;
using KY.Generator.Configuration;
using KY.Generator.Configurations;
using KY.Generator.Output;
using KY.Generator.Transfer;
using KY.Generator.Transfer.Writers;

namespace KY.Generator.Client
{
    internal class GeneratorGenerator : Codeable, ITransferWriter
    {
        public void Write(ConfigurationBase configurationBase, List<ITransferObject> transferObjects, IOutput output)
        {
            Logger.Trace("Generate generator...");
            GeneratorConfiguration configuration = (GeneratorConfiguration)configurationBase;
        }
    }
}