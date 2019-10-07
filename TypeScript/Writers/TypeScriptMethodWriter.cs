using System.Linq;
using KY.Generator.Models;
using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.Writers;

namespace KY.Generator.TypeScript.Writers
{
    public class TypeScriptMethodWriter : ITemplateWriter
    {
        public virtual void Write(ICodeFragment fragment, IOutputCache output)
        {
            MethodTemplate template = (MethodTemplate)fragment;
            output.Add(template.Comment)
                  .Add(template.Attributes)
                  .If(template.Visibility != Visibility.None).Add(template.Visibility.ToString().ToLower()).Add(" ").EndIf()
                  .If(template.IsStatic).Add("static ").EndIf()
                  .If(template.IsOverride).Add("override ").EndIf()
                  .Add(template.Name)
                  .Add("(")
                  .Add(template.Parameters.OrderBy(x => x.DefaultValue == null ? 0 : 1), ", ")
                  .Add(")")
                  .If(template.Type != null).Add(": ").Add(template.Type).EndIf()
                  .StartBlock()
                  .Add(template.Code)
                  .EndBlock();
        }
    }
}