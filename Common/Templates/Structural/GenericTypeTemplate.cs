namespace KY.Generator.Templates;

public class GenericTypeTemplate : TypeTemplate
{
    public List<TypeTemplate> Types { get; } = new();

    public GenericTypeTemplate(string name, string? nameSpace = null, bool isNullable = false, bool fromSystem = false, bool isInterface = false)
        : base(name, nameSpace, isInterface, isNullable, fromSystem)
    { }

    protected GenericTypeTemplate()
    { }
}
