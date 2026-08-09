using KY.Generator;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace EdgeCases;

/// <summary>
/// Covers excluding single members and whole types from the generation.
/// </summary>
[GenerateTypeScriptModel("Output")]
public class IgnoredMembers
{
    public string UntouchedProperty { get; set; } = "";

    [GenerateIgnore]
    public string IgnoredProperty { get; set; } = "";

    [GenerateIgnore]
    public IgnoredType IgnoredTypeProperty { get; set; } = new();

    [GenerateIgnore]
    public string IgnoredField = "";
}

/// <summary>
/// Only referenced through an ignored property, so it must not be generated at all.
/// </summary>
public class IgnoredType
{
    public string StringProperty { get; set; } = "";
}

/// <summary>
/// The type itself is excluded even though it is annotated.
/// </summary>
[GenerateIgnore]
[GenerateTypeScriptModel("Output")]
public class CompletelyIgnoredType
{
    public string StringProperty { get; set; } = "";
}
