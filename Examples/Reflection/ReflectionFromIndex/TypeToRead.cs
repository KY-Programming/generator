using KY.Generator;

namespace ReflectionFromIndex;

/// <summary>
/// One of the two models that end up in Output - both are re-exported from the generated index.ts, so a
/// consumer can import them from the folder instead of from the single files.
/// </summary>
[GenerateTypeScriptModel("Output")]
public class TypeToRead
{
    public string StringProperty { get; set; } = "";
    public int NumberProperty { get; set; }
}
