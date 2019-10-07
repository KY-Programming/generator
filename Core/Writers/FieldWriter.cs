using System.Linq;
using KY.Generator.Languages;
using KY.Generator.Models;
using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    public class FieldWriter : ITemplateWriter
    {
        protected BaseLanguage Language { get; }

        public FieldWriter(BaseLanguage language)
        {
            this.Language = language;
        }

        public virtual void Write(ICodeFragment fragment, IOutputCache output)
        {
            FieldTemplate template = (FieldTemplate)fragment;
            FieldTemplate lastTemplate = output.LastFragments.FirstOrDefault() as FieldTemplate;
            if (template.Attributes.Count > 0 || lastTemplate?.Attributes.Count > 0)
            {
                output.BreakLine();
            }
            output.Add(template.Attributes)
                  .If(template.Visibility != Visibility.None).Add(template.Visibility.ToString().ToLower()).Add(" ").EndIf()
                  .If(template.IsStatic).Add("static ").EndIf()
                  .If(template.IsConst).Add("const ").EndIf()
                  .If(template.IsReadonly).Add("readonly ").EndIf()
                  .Add(template.Type).Add(" ")
                  .Add(template.Name)
                  .If(template.DefaultValue != null).Add(" = ").Add(template.DefaultValue).EndIf()
                  .CloseLine();
        }
    }
}