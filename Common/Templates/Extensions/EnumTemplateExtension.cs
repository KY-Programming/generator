namespace KY.Generator.Templates.Extensions;

public static class EnumTemplateExtension
{
    public static EnumTemplate FormatName(this EnumTemplate enumTemplate, GeneratorOptions options, bool force = false)
    {
        enumTemplate.Name = Formatter.FormatClass(enumTemplate.Name, options, force);
        return enumTemplate;
    }

    public static EnumTemplate AddValue(this EnumTemplate enumTemplate, string name, int? value = null)
    {
        enumTemplate.Values.Add(new EnumValueTemplate(name, Code.Instance.Number(value ?? enumTemplate.Values.Count)));
        return enumTemplate;
    }

    public static TypeTemplate ToType(this EnumTemplate enumTemplate)
    {
        return Code.Instance.Type(enumTemplate.Name);
    }

    public static UsingTemplate ToUsing(this EnumTemplate enumTemplate)
    {
        return new UsingTemplate(enumTemplate.Namespace.Name, enumTemplate.Name, enumTemplate.Namespace.File.RelativePath);
    }
}