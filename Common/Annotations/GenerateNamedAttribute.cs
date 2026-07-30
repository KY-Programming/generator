namespace KY.Generator;

/// <summary>
/// Base class for generation attributes that can influence the name of the generated element.
/// Carries the shared options to rename an element or to substring-replace parts of its name.
/// </summary>
public abstract class GenerateNamedAttribute : Attribute
{
    /// <summary>
    /// Renames the element in the generated output. The name replaces the original name and therefore
    /// bypasses <see cref="Replace"/>, but it is still processed by the configured name formatting
    /// (casing) and by a configured class or interface prefix.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Substring in the element name to replace. Used together with <see cref="With"/>.
    /// Ignored when <see cref="Name"/> is set.
    /// </summary>
    public string? Replace { get; set; }

    /// <summary>
    /// Replacement substring for <see cref="Replace"/>. Defaults to empty string when omitted.
    /// </summary>
    public string? With { get; set; }
}
