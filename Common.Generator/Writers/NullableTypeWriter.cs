using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator.Writers;

/// <summary>
/// Default writer for languages that do not render the nullability of a nested type. Writes the wrapped type only
/// </summary>
public class NullableTypeWriter : ITemplateWriter
{
    public virtual void Write(ICodeFragment fragment, IOutputCache output)
    {
        NullableTypeTemplate template = (NullableTypeTemplate)fragment;
        output.Add(template.Type);
    }
}
