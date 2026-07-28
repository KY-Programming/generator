namespace KY.Generator;

/// <summary>
/// Base class for generation attributes that can influence the name of the generated element.
/// Carries the shared options to rename an element or to substring-replace parts of its name.
/// </summary>
public abstract class GenerateNamedAttribute : Attribute
{
    /// <summary>
    /// Renames the element to this exact name in the generated output. The name is used as given
    /// and bypasses the language name formatting and <see cref="Replace"/>, but a configured
    /// class or interface prefix is still applied.
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
