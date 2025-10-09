using System.Diagnostics;
using KY.Generator.Models;

namespace KY.Generator.Templates;

[DebuggerDisplay("Method {Name}")]
public class MethodTemplate : AttributeableTempalte
{
    public string Name { get; set; }
    public TypeTemplate? Type { get; }
    public Visibility Visibility { get; set; } = Visibility.Public;
    public bool IsStatic { get; set; }
    public bool IsOverride { get; set; }
    public List<ParameterTemplate> Parameters { get; } = [];
    public MultilineCodeFragment Code { get; } = new();
    public ClassTemplate Class { get; }
    public CommentTemplate? Comment { get; set; }
    public List<MethodGenericTemplate> Generics { get; set; } = [];

    public MethodTemplate(ClassTemplate classTemplate, string name, TypeTemplate? type)
    {
        this.Class = classTemplate;
        this.Name = name;
        this.Type = type;
    }

    public MethodTemplate(MethodTemplate methodTemplate)
        : this(methodTemplate.Class, methodTemplate.Name, methodTemplate.Type)
    {
        this.Visibility = methodTemplate.Visibility;
        this.IsStatic = methodTemplate.IsStatic;
        this.IsOverride = methodTemplate.IsOverride;
        this.Parameters.AddRange(methodTemplate.Parameters);
        this.Code = methodTemplate.Code;
        this.Comment = methodTemplate.Comment;
        this.Generics.AddRange(methodTemplate.Generics);
    }
}
