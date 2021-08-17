using System.Collections.Generic;
using KY.Generator.Output;
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

        public void Write(List<ITransferObject> transferObjects, string relativePath, IOutput output)
        {
            this.modelWriter.Write(transferObjects, relativePath, output);
        }
    }
}
