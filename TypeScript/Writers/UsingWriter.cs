using KY.Core;
using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.TypeScript.Templates;
using KY.Generator.Writers;

namespace KY.Generator.TypeScript.Writers
{
    public class UsingWriter : ITemplateWriter
    {
        private readonly IOptions options;

        public UsingWriter(IOptions options)
        {
            this.options = options;
        }

        public virtual void Write(ICodeFragment fragment, IOutputCache output)
        {
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
                  .Add(this.options.Formatting.Quote)
                  .Add(template.Path.TrimEnd(".ts"))
                  .Add(this.options.Formatting.Quote)
                  .CloseLine();
        }
    }
}
