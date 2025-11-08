namespace KY.Generator.Templates.Extensions;

public static class NamespaceExtension
{
    public static ClassTemplate AddClass(this NamespaceTemplate namespaceTemplate, string name, params TypeTemplate[] basedOn)
    {
        ClassTemplate classTemplate = new(namespaceTemplate, name, basedOn);
        namespaceTemplate.Children.Add(classTemplate);
        return classTemplate;
    }

    public static EnumTemplate AddEnum(this NamespaceTemplate namespaceTemplate, string name, TypeTemplate? basedOn = null)
    {
        EnumTemplate enumTemplate = new(namespaceTemplate, name, basedOn);
        namespaceTemplate.Children.Add(enumTemplate);
        return enumTemplate;
    }
}
