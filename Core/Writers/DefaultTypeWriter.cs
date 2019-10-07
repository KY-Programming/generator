using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    public class DefaultTypeWriter : ITypeWriter, IGenericTypeWriter
    {
        public string NullableFormatter { get; }

        public DefaultTypeWriter(string nullableFormatter = "{0}?")
        {
            this.NullableFormatter = nullableFormatter;
        }

        public void Write(TypeTemplate template, IOutputCache output)
        {
            if (template.IsNullable)
            {
                output.Add(string.Format(this.NullableFormatter, template.Name));
            }
            else
            {
                output.Add(template.Name);
            }
        }

        public void Write(GenericTypeTemplate template, IOutputCache output)
        {
            output.Add(template.Name)
                  .Add("<")
                  .Add(template.Types, ", ")
                  .Add(">");
        }
    }
}