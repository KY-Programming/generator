using KY.Core;
using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.TypeScript.Templates;
using KY.Generator.Writers;

namespace KY.Generator.TypeScript.Writers;

public class UsingWriter : ITemplateWriter
{
    private readonly Options options;

    public UsingWriter(Options options)
    {
        this.options = options;
    }

    public virtual void Write(ICodeFragment fragment, IOutputCache output)
    {
        GeneratorOptions generatorOptions = this.options.Get<GeneratorOptions>();
        UsingTemplate template = (UsingTemplate)fragment;
        if (template is UnknownExportTemplate unknownUsing)
        {
            output.Add(unknownUsing.Code).BreakLine();
            return;
        }
        if (template.Path == null || template.Type == null)
        {
            Logger.Error("Invalid TypeScript import/export (path or type is missing)");
            return;
        }
        string action = template is ExportTemplate ? "export" : "import";
        string typeName = template.Type;
        if (!typeName.StartsWith("*"))
        {
            typeName = $"{{ {typeName} }}";
        }
        output.Add($"{action} {typeName} from ")
              .Add(generatorOptions.Formatting.Quote)
              .Add(template.Path.TrimEnd(".ts"))
              .Add(generatorOptions.Formatting.Quote)
              .CloseLine();
    }
}
