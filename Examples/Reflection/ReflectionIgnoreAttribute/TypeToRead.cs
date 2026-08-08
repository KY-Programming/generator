using KY.Generator;

namespace ReflectionIgnoreAttribute;

/// <summary>
/// The only type written in full. It collects every variant of [GenerateIgnore]: on a field, on a
/// property, and on the types behind them.
///
/// A member may not simply use an ignored type: the member would be generated with an import of a file
/// that is never written, so the generation is aborted with an error. There are two ways out, both shown
/// below - leave the member out with [GenerateIgnore], or bind the type to a hand written file with
/// [GenerateImport], as done here for <see cref="TypeToIgnore"/>.
/// </summary>
[GenerateTypeScriptModel("Output")]
[GenerateImport(typeof(TypeToIgnore), "./type-to-ignore", nameof(TypeToIgnore))]
public class TypeToRead
{
    /// <summary>
    /// This field will be ignored
    /// </summary>
    [GenerateIgnore]
    public string StringFieldToIgnore = "";

    /// <summary>
    /// This field will be ignored
    /// </summary>
    [GenerateIgnore]
    public FieldTypeToIgnore FieldTypeFieldToIgnore = new();

    /// <summary>
    /// This field is written to output, the class is not: [GenerateImport] on TypeToRead points the
    /// import at the hand written Output/type-to-ignore.ts
    /// </summary>
    public TypeToIgnore TypeToIgnoreField = new();

    public string StringProperty { get; set; } = "";
    public int NumberProperty { get; set; }

    /// <summary>
    /// This property will be ignored
    /// </summary>
    [GenerateIgnore]
    public string StringPropertyToIgnore { get; set; } = "";

    /// <summary>
    /// This property will be ignored
    /// </summary>
    [GenerateIgnore]
    public PropertyTypeToIgnore PropertyTypeToIgnore { get; set; } = new();

    /// <summary>
    /// This property is written to output, the class is not - same [GenerateImport] as the field above
    /// </summary>
    public TypeToIgnore TypeToIgnoreProperty { get; set; } = new();

    /// <summary>
    /// The other way out: the enum behind this property is ignored and no import is bound for it, so the
    /// property itself has to be left out - without [GenerateIgnore] the generation would fail here
    /// </summary>
    [GenerateIgnore]
    public EnumToIgnore EnumToIgnoreProperty { get; set; }
}