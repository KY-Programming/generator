using KY.Generator.Models;
using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.Writers;

namespace KY.Generator.TypeScript.Writers;

public class TypeScriptFieldWriter : ITemplateWriter
{
    public virtual void Write(ICodeFragment fragment, IOutputCache output)
    {
        FieldTemplate template = (FieldTemplate)fragment;
        output.If(template.Visibility != Visibility.None).Add(template.Visibility.ToString().ToLower()).Add(" ").EndIf()
              .If(template.IsStatic || template.IsConstant).Add("static ").EndIf()
              .If(template.IsReadonly || template.IsConstant).Add("readonly ").EndIf()
              .Add(template.Name)
              .If(template.IsOptional).Add("?").EndIf()
              .Add(": ")
              .Add(template.Type)
              .If(template.Strict && template.IsNullable).Add(" | undefined").EndIf()
              .If(template.DefaultValue != null && !template.Class.IsInterface).Add(" = ").Add(template.DefaultValue!).EndIf()
              .CloseLine();
    }
}