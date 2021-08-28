using System.Linq;
using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.Writers;

namespace KY.Generator.TypeScript.Writers
{
    public class TypeScriptEnumWriter : EnumWriter
    {
        public TypeScriptEnumWriter(IOptions options)
            : base(options)
        { }

        public override void Write(ICodeFragment fragment, IOutputCache output)
        {
            EnumTemplate template = (EnumTemplate)fragment;
            template.BasedOn = null;
            base.Write(fragment, output);

            output.BreakLine()
                  .Add($"export const {template.Name}Values = [").Add(template.Values.Select(x => x.Value), ", ").Add("]").CloseLine()
                  .Add($"export const {template.Name}Names = [").Add(template.Values.Select(x => Code.String(x.Name)), ", ").Add("]").CloseLine()
                  .Add($"export const {template.Name}ValueMapping: {{ [key: number]: string }} = {{ ");
            bool isFirst = true;
            foreach (EnumValueTemplate value in template.Values)
            {
                if (isFirst)
                {
                    isFirst = false;
                }
                else
                {
                    output.Add(", ");
                }
                output.Add(value.Value).Add(": ").Add(Code.String(value.Name));
            }
            output.Add(" }").CloseLine()
                  .Add($"export const {template.Name}NameMapping: {{ [key: string]: number }} = {{ ");
            isFirst = true;
            foreach (EnumValueTemplate value in template.Values)
            {
                if (isFirst)
                {
                    isFirst = false;
                }
                else
                {
                    output.Add(", ");
                }
                output.Add(Code.String(value.Name)).Add(": ").Add(value.Value);
            }
            output.Add(" }").CloseLine();
        }
    }
}
