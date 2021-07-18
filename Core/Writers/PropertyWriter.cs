using System.Linq;
using KY.Generator.Models;
using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    public class PropertyWriter : ITemplateWriter
    {
        public virtual void Write(ICodeFragment fragment, IOutputCache output)
        {
            PropertyTemplate template = (PropertyTemplate)fragment;
            PropertyTemplate previousProperty = output.LastFragments.TakeWhile(x => !(x is ClassTemplate)).OfType<PropertyTemplate>().Skip(1).FirstOrDefault();
            if (previousProperty?.Attributes.Count > 0 || previousProperty != null && template.Attributes.Count > 0)
            {
                output.BreakLine();
            }
            if (template.Comment != null && !string.IsNullOrWhiteSpace(template.Comment.Description))
            {
                output.Add(template.Comment).BreakLine();
            }
            if (template.Attributes.Count > 0)
            {
                output.Add(template.Attributes);
            }
            output.Add(template.Visibility == Visibility.None ? string.Empty : template.Visibility.ToString().ToLower())
                  .Add(" ")
                  .Add(template.IsVirtual ? "virtual " : string.Empty)
                  .Add(template.IsStatic ? "static " : string.Empty)
                  .Add(template.Type)
                  .Add(" ")
                  .Add(template.Name);
            if (template.HasGetter || template.HasSetter)
            {
                output.Add(" { ")
                      .Add(template.HasGetter ? "get; " : string.Empty)
                      .Add(template.HasSetter ? "set; " : string.Empty)
                      .Add("}");
            }
            if (template.DefaultValue != null)
            {
                output.Add(" = ")
                      .Add(template.DefaultValue)
                      .CloseLine();
            }
            else if (template.Expression != null)
            {
                output.Add(" => ")
                      .Add(template.Expression)
                      .CloseLine();
            }
            else if (template.Getter != null || template.Setter != null)
            {
                if (template.Getter is MultilineCodeFragment multilineGetter)
                {
                    output.Add("get").BreakLine()
                          .Add("{").BreakLine()
                          .Add(multilineGetter).BreakLine()
                          .Add("}");
                }
                else if (template.Getter != null)
                {
                    output.Add("get => ")
                          .Add(template.Getter);
                }
                if (template.Setter is MultilineCodeFragment multilineSetter)
                {
                    output.Add("set").BreakLine()
                          .Add("{").BreakLine()
                          .Add(multilineSetter).BreakLine()
                          .Add("}");
                }
                else if (template.Getter != null)
                {
                    output.Add("set => ")
                          .Add(template.Getter);
                }
            }
            output.BreakLineIfNotEmpty();
        }
    }
}
