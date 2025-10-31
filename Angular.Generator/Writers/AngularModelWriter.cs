using KY.Generator.Mappings;
using KY.Generator.Templates;
using KY.Generator.Transfer;
using KY.Generator.TypeScript.Transfer;

namespace KY.Generator.Angular.Writers;

public class AngularModelWriter : TypeScriptModelWriter
{
    public AngularModelWriter(Options options, ITypeMapping typeMapping, IEnumerable<ITransferObject> transferObjects, IList<FileTemplate> files)
        : base(options, typeMapping, transferObjects, files)
    { }

    protected override void WriteModel(ModelTransferObject model)
    {
        GeneratorOptions options = this.Options.Get<GeneratorOptions>(model);
        options.ModelOutput ??= "/ClientApp/src/app/models";
        base.WriteModel(model);
    }
}
