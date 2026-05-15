namespace KY.Generator;

/// <summary>
/// Base class for member-level generation attributes. Carries shared options to
/// rename a member, override its generated type, or substring-replace its name.
/// </summary>
public abstract class GenerateMemberAttribute : Attribute
{
    /// <summary>
    /// Renames the member to this exact name in the generated output.
    /// </summary>
    public string? Name { get; set; }

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

    /// <summary>
    /// Substring in the member name to replace. Used together with <see cref="With"/>.
    /// </summary>
    public string? Replace { get; set; }

    /// <summary>
    /// Replacement substring for <see cref="Replace"/>. Defaults to empty string when omitted.
    /// </summary>
    public string? With { get; set; }
}
