using System.Linq;
using KY.Generator.Models;
using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    public class PropertyWriter : ITemplateWriter
    {
        /// <summary>
        /// Written directly behind the type of a nullable member. Languages that express nullability at the place
        /// the type is used override this - C# writes its "?" here. Nothing by default, so a language that does not
        /// know the concept is unaffected.
        /// <para>
        /// The type is asked, not the member: the type mapping already decided whether the target type can carry a
        /// null annotation at all (its "Nullable()" flag), so a target that can not - a C# reference type outside a
        /// nullable context - never reaches this.
        /// </para>
        /// </summary>
        protected virtual void WriteNullableMarker(TypeTemplate type, IOutputCache output)
        { }

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
                  .Add(template.Type);
            this.WriteNullableMarker(template.Type, output);
            output.Add(" ")
                  .Add(template.Name);
            if (template.HasGetter || template.HasSetter)
            {
                output.Add(" { ")
                      .Add(template.HasGetter ? "get; " : string.Empty)
                      .Add(template.HasSetter ? "set; " : string.Empty)
                      .Add("}");
            }
            if (template.DefaultValue != null && !template.Class.IsInterface)
            {
                output.Add(" = ")
                      .Add(template.DefaultValue)
                      .CloseLine();
            }
            else if (template.Expression != null && !template.Class.IsInterface)
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
