using KY.Generator;
using SecondAssembly;

namespace MainAssembly;

/// <summary>
/// Only this type is annotated. <see cref="SecondType"/> comes from the referenced SecondAssembly and is
/// generated as well, because it is reachable from a generated type.
/// </summary>
[GenerateTypeScriptModel("Output")]
public class TypeToRead
{
    public string StringProperty { get; set; } = "";
    public int NumberProperty { get; set; }
    public SecondType SecondTypeProperty { get; set; } = new();
}
