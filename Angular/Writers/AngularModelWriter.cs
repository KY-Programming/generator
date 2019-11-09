using System.Collections.Generic;
using KY.Generator.Angular.Configurations;
using KY.Generator.Templates;
using KY.Generator.Transfer;
using KY.Generator.Transfer.Writers;
using KY.Generator.TypeScript.Transfer;

namespace KY.Generator.Angular.Writers
{
    public class AngularModelWriter
    {
        private readonly ModelWriter modelWriter;

        public AngularModelWriter(TypeScriptModelWriter modelWriter)
        {
            this.modelWriter = modelWriter;
        }

        public void Write(AngularWriteConfiguration configuration, List<ITransferObject> transferObjects, List<FileTemplate> files)
        {
            if (configuration.Model != null)
            {
                configuration.Model.FormatNames = configuration.FormatNames;
                this.modelWriter.Write(configuration.Model, transferObjects).ForEach(files.Add);
            }
        }
    }
}