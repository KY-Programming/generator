using System.Linq;
using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    public class MethodWriter : ITemplateWriter
    {
        public virtual void Write(ICodeFragment fragment, IOutputCache output)
        {
            MethodTemplate template = (MethodTemplate)fragment;
            output.Add(template.Comment)
                  .Add(template.Attributes)
                  .Add(template.Visibility.ToString().ToLower()).Add(" ")
                  .If(template.IsStatic).Add("static ").EndIf()
                  .If(template.IsOverride).Add("override ").EndIf()
                  .If(template.Type != null).Add(template.Type).Add(" ").EndIf()
                  .Add(template.Name)
                  .Add("(")
                  .If(template is ExtensionMethodTemplate).Add("this ").EndIf()
                  .Add(template.Parameters.OrderBy(x => x.DefaultValue == null ? 0 : 1), ", ")
                  .Add(")");
            this.BeforeBlock(fragment, output);
            output.StartBlock()
                  .Add(template.Code)
                  .EndBlock();
        }

        protected virtual void BeforeBlock(ICodeFragment fragment, IOutputCache output)
        { }
    }
}