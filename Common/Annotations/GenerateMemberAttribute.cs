namespace KY.Generator;

/// <summary>
/// Base class for member-level generation attributes. Carries shared options to
/// rename a member, override its generated type, or substring-replace its name.
/// </summary>
public abstract class GenerateMemberAttribute : GenerateNamedAttribute
{
    /// <summary>
    /// Overrides the generated type of the member (property/field type, method return type).
    /// </summary>
    public Type? Type { get; set; }

    /// <summary>
    /// Overrides the generated type of the member by name, when no matching C# type exists
    /// (e.g. an ambient TypeScript type or an array form like <c>"Foo[]"</c>).
    /// Ignored when <see cref="Type"/> is also set. The import for this type must be
    /// declared separately via <see cref="GenerateImportAttribute"/>, or the type must
    /// already be in scope in the target language.
    /// </summary>
    public string? TypeName { get; set; }
}
