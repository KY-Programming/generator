using System.Collections.Generic;
using KY.Core;
using KY.Generator.Command;
using KY.Generator.Configuration;
using KY.Generator.Extensions;
using KY.Generator.Templates;
using KY.Generator.Transfer;
using KY.Generator.Transfer.Writers;

namespace KY.Generator.Commands
{
    public class ModelWriteCommand : IConfigurationCommand
    {
        private readonly ModelWriter writer;

        public ModelWriteCommand(ModelWriter writer)
        {
            this.writer = writer;
        }

        public bool Execute(IConfiguration configurationBase, List<ITransferObject> transferObjects)
        {
            Logger.Trace("Write model...");
            configurationBase.Output.AssertIsNotNull(message: $"The output of '{configurationBase.Environment.Command}' command is not set");
            IModelConfiguration configuration = (IModelConfiguration)configurationBase;
            List<FileTemplate> files = this.writer.Write(configuration, transferObjects);
            files.Write(configuration);
            return true;
        }
    }
}