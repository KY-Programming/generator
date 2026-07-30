using System.Diagnostics;

namespace KY.Generator.Templates;

/// <summary>
/// Wraps a type that is used in a nullable position, e.g. the element type of <c>List&lt;string?&gt;</c>. The
/// nullability is rendered by the language specific writer, everything else is delegated to the wrapped
/// <see cref="Type"/> so writers that do not know this template behave as if the type was not nullable
/// </summary>
[DebuggerDisplay("NullableTypeTemplate: {Name}")]
public class NullableTypeTemplate : TypeTemplate
{
    public TypeTemplate Type { get; }

    /// <summary>
    /// Only in strict mode the nullability is part of the written type. Set by the writer that knows the member
    /// this type belongs to
    /// </summary>
    public bool Strict { get; set; }

    public override string Name => this.Type.Name;
    public override string? Namespace => this.Type.Namespace;
    public override bool IsInterface => this.Type.IsInterface;
    public override bool IsNullable => true;
    public override bool FromSystem => this.Type.FromSystem;

    public NullableTypeTemplate(TypeTemplate type)
    {
        this.Type = type;
    }
}
