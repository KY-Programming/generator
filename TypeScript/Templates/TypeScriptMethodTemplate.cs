using KY.Generator.Templates;

namespace KY.Generator.TypeScript.Templates;

public class TypeScriptMethodTemplate : MethodTemplate
{
    public List<TypeScriptMethodOverloadTemplate> Overloads { get; } = [];

    public TypeScriptMethodTemplate(ClassTemplate classTemplate, string name, TypeTemplate type)
        : base(classTemplate, name, type)
    { }

    public TypeScriptMethodTemplate(MethodTemplate methodTemplate)
        : base(methodTemplate)
    { }
}

public class TypeScriptMethodOverloadTemplate
{
    public TypeTemplate ReturnType { get; set; }
    public List<ParameterTemplate> Parameters { get; } = [];
    public CommentTemplate? Comment { get; set; }
    public List<MethodGenericTemplate> Generics { get; set; } = [];
}
