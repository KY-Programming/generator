using System.Collections.Generic;
using System.Linq;
using KY.Generator.Transfer;
using KY.Generator.TypeScript.Transfer;

namespace KY.Generator.Angular.Writers;

public class AngularModelWriter(TypeScriptModelWriter modelWriter, IEnumerable<ITransferObject> transferObjects, Options options)
{
    public AngularModelWriter FormatNames()
    {
        modelWriter.FormatNames();
        return this;
    }

    public void Write(string relativePath)
    {
        foreach (ModelTransferObject model in transferObjects.OfType<ModelTransferObject>())
        {
            AngularOptions angularOptions = options.Get<AngularOptions>(model);
            modelWriter.Write(relativePath ?? angularOptions.ModelOutput ?? "/ClientApp/src/app/models");
        }
    }
}
