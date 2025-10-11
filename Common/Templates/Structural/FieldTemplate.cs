using System.Diagnostics;
using KY.Generator.Models;

namespace KY.Generator.Templates;

[DebuggerDisplay("Field {Name}: {Type}")]
public class FieldTemplate : AttributeableTempalte
{
    public string Name { get; set; }
    public TypeTemplate Type { get; }
    public bool IsStatic { get; set; }
    public bool IsConstant { get; set; }
    public Visibility Visibility { get; set; }
    public ICodeFragment? DefaultValue { get; set; }
    public ClassTemplate Class { get; }
    public CommentTemplate? Comment { get; set; }
    public bool IsReadonly { get; set; }
    public bool IsOptional { get; set; }

    public bool IsNullable { get; set; }

    // TODO: Remove temporary property Strict
    public bool Strict { get; set; }

    public FieldTemplate(ClassTemplate classTemplate, string name, TypeTemplate type)
    {
        this.Class = classTemplate;
        this.Name = name;
        this.Type = type;
        this.Visibility = Visibility.Private;
    }

    public override bool Equals(object? obj)
    {
        FieldTemplate? property = obj as FieldTemplate;
        return property != null && this.Name.Equals(property.Name);
    }

    public override int GetHashCode()
    {
        return this.Name.GetHashCode();
    }
}
