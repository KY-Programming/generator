using KY.Generator;

namespace CasingAnnotations;

/// <summary>
/// The same members as CaseMe, but GenerateFormatNames(false) opts this type out of the name formatting,
/// so every one of them has to reach the output with the spelling it has here - underscores and all.
/// This attribute is what the fluent twin expresses as SetType&lt;KeepMyCase&gt;(c => c.FormatNames(false)).
/// </summary>
[GenerateFormatNames(false)]
public class KeepMyCase
{
    public string alllower { get; set; } = "";
    public string ALLUPPER { get; set; } = "";
    public string PascalCase { get; set; } = "";
    public string camelCase { get; set; } = "";
    public string snake_case { get; set; } = "";
    public string UPPER_SNAKE_CASE { get; set; } = "";
    public string S1 { get; set; } = "";
}
