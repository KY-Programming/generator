using KY.Generator.Models;
using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;
using KY.Generator.Writers;

namespace KY.Generator.TypeScript.Writers
{
    public class TypeScriptPropertyWriter : Codeable, ITemplateWriter
    {
        public virtual void Write(ICodeFragment fragment, IOutputCache output)
        {
            PropertyTemplate template = (PropertyTemplate)fragment;
            FieldTemplate fieldTemplate = new(template.Class, template.Name, template.Type);
            fieldTemplate.Strict = template.Strict;
            if (template.Getter == null && template.HasGetter || template.Setter == null && template.HasSetter)
            {
                if (fieldTemplate.Name == template.Name)
                {
                    fieldTemplate.Name += "Field";
                }
                fieldTemplate.DefaultValue = template.DefaultValue;
                output.Add(fieldTemplate);
            }
            if (template.HasGetter || template.Getter != null)
            {
                output.If(template.Visibility != Visibility.None).Add(template.Visibility.ToString().ToLower()).Add(" ").EndIf()
                      .If(template.IsStatic).Add("static ").EndIf()
                      .Add($"get {template.Name}(): ")
                      .Add(template.Type)
                      .If(template.Strict && template.Type.IsNullable).Add(" | undefined").EndIf()
                      .StartBlock()
                      .Add(template.Getter ?? Code.Return(Code.This().Field(fieldTemplate.Name)))
                      .EndBlock();
            }
            if (template.HasSetter || template.Setter != null)
            {
                output.If(template.Visibility != Visibility.None).Add(template.Visibility.ToString().ToLower()).Add(" ").EndIf()
                      .If(template.IsStatic).Add("static ").EndIf()
                      .Add($"set {template.Name}(value: ")
                      .Add(template.Type)
                      .If(template.Strict && template.Type.IsNullable).Add(" | undefined").EndIf()
                      .Add(")")
                      .StartBlock()
                      .Add(template.Setter ?? Code.This().Field(fieldTemplate.Name).Assign(Code.Local("value")).Close())
                      .EndBlock();
            }
        }
    }
}
