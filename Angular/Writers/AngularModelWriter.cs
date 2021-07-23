using System.Collections.Generic;
using System.Linq;
using KY.Generator.Angular.Configurations;
using KY.Generator.Templates;
using KY.Generator.Transfer;
using KY.Generator.TypeScript.Transfer;

namespace KY.Generator.Angular.Writers
{
    public class AngularModelWriter
    {
        private readonly TypeScriptModelWriter modelWriter;

        public AngularModelWriter(TypeScriptModelWriter modelWriter)
        {
            this.modelWriter = modelWriter;
        }

        public void Write(AngularWriteConfiguration configuration, List<ITransferObject> transferObjects, List<FileTemplate> files)
        {
            if (configuration.Model != null)
            {
                configuration.Model.FormatNames = configuration.FormatNames;
                configuration.Model.OutputId = configuration.OutputId;
                this.modelWriter.Write(configuration.Model, transferObjects).ForEach(files.Add);
            }
        }
    }
}
