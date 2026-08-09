using System.Linq;
using KY.Generator.Languages;
using KY.Generator.Models;
using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    public class FieldWriter : ITemplateWriter
    {
        /// <summary>See <see cref="PropertyWriter.WriteNullableMarker"/> - the same for fields.</summary>
        protected virtual void WriteNullableMarker(TypeTemplate type, IOutputCache output)
        { }

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
                  .If(template.IsConstant).Add("const ").EndIf()
                  .If(template.IsReadonly).Add("readonly ").EndIf()
                  .Add(template.Type);
            this.WriteNullableMarker(template.Type, output);
            output.Add(" ")
                  .Add(template.Name)
                  .If(template.DefaultValue != null && !template.Class.IsInterface).Add(" = ").Add(template.DefaultValue).EndIf()
                  .CloseLine();
        }
    }
}
