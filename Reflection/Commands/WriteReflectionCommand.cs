using System;
using System.Collections.Generic;
using KY.Core;
using KY.Generator.Command;
using KY.Generator.Configuration;
using KY.Generator.Extensions;
using KY.Generator.Reflection.Configurations;
using KY.Generator.Templates;
using KY.Generator.Transfer;
using KY.Generator.Transfer.Writers;

namespace KY.Generator.Reflection.Commands
{
    internal class WriteReflectionCommand : IConfigurationCommand
    {
        private readonly ModelWriter modelWriter;

        public WriteReflectionCommand(ModelWriter modelWriter)
        {
            this.modelWriter = modelWriter;
        }

        public bool Execute(IConfiguration configurationBase, List<ITransferObject> transferObjects)
        {
            Logger.Trace("Write reflection...");
            WriteReflectionConfiguration configuration = (WriteReflectionConfiguration)configurationBase;
            if (configuration.Language == null)
            {
                throw new InvalidOperationException($"Can not generate Reflection.Type for language {configuration.Language?.Name ?? "Empty"}");
            }
            List<FileTemplate> files = this.modelWriter.Write(configuration, transferObjects);
            files.Write(configuration);
            return true;
        }
    }
}