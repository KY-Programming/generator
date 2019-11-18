using System;
using System.Collections.Generic;
using KY.Core;
using KY.Generator.Configurations;
using KY.Generator.Output;
using KY.Generator.Reflection.Configurations;
using KY.Generator.Templates;
using KY.Generator.Transfer;
using KY.Generator.Transfer.Writers;

namespace KY.Generator.Reflection.Writers
{
    internal class ReflectionWriter : ITransferWriter
    {
        private readonly ModelWriter modelWriter;

        public ReflectionWriter(ModelWriter modelWriter)
        {
            this.modelWriter = modelWriter;
        }

        public void Write(ConfigurationBase configurationBase, List<ITransferObject> transferObjects, IOutput output)
        {
            Logger.Trace("Generate reflection...");
            ReflectionWriteConfiguration configuration = (ReflectionWriteConfiguration)configurationBase;
            if (configuration.Language == null)
            {
                throw new InvalidOperationException($"Can not generate Reflection.Type for language {configuration.Language?.Name ?? "Empty"}");
            }
            List<FileTemplate> files = this.modelWriter.Write(configuration, transferObjects);
            files.ForEach(file => configuration.Language.Write(file, output));
        }
    }
}