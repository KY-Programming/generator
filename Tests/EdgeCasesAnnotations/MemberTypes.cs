using KY.Generator;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace EdgeCasesAnnotations;

/// <summary>
/// Covers the member level annotations: renaming a member, renaming via replace and overriding the
/// generated type.
/// </summary>
[GenerateTypeScriptModel("Output")]
public class MemberTypes
{
    public string UntouchedProperty { get; set; } = "";

    [GenerateProperty(Name = "renamedProperty")]
    public string OriginalNameProperty { get; set; } = "";

    [GenerateProperty(Replace = "Ugly", With = "Nice")]
    public string UglyNameProperty { get; set; } = "";

    [GenerateProperty(TypeName = "string")]
    public int TypeNameOverrideProperty { get; set; }

    [GenerateField(Name = "renamedField")]
    public string OriginalNameField = "";
}
