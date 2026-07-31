using KY.Generator;

namespace Types;

[GenerateTypeScriptModel]
public class NullableEnabledTypes
{
    public string? NullableString { get; set; }
    public string NonNullableString { get; set; } = string.Empty;
    public NullableDisabledTypes NullableDisabledTypes { get; set; } = new();
}
