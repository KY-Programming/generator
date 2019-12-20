using System.Collections.Generic;
using KY.Core;
using KY.Generator.Configurations;
using KY.Generator.Output;

namespace KY.Generator.Transfer.Writers
{
    public class DemoWriter : ITransferWriter
    {
        public void Write(ConfigurationBase configurationBase, List<ITransferObject> transferObjects, IOutput output)
        {
            DemoConfiguration configuration = (DemoConfiguration)configurationBase;
            Logger.Trace(configuration.Message);

            Logger.Trace("See full documentation on https://github.com/KY-Programming/generator/wiki");
        }
    }
}