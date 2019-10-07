using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.Writers;

namespace KY.Generator.TypeScript.Writers
{
    public class AnonymousObjectWriter : ITemplateWriter
    {
        public virtual void Write(ICodeFragment fragment, IOutputCache output)
        {
            AnonymousObjectTemplate template = (AnonymousObjectTemplate)fragment;
            output.Add("{")
                  .Indent();
            foreach (PropertyValueTemplate property in template.Properties)
            {
                output.Add($"{property.Name}: ")
                      .Add(property.Value)
                      .BreakLine();
            }
            output.UnIndent().Add("}");
        }
    }
}