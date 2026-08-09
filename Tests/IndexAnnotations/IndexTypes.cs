using KY.Generator;

namespace Index;

/// <summary>
/// Several models in one output folder so the generated index.ts has more than one export to barrel up.
/// The sub folder additionally checks that a nested output directory gets its own index.ts.
/// </summary>
[GenerateTypeScriptModel]
public class IndexTypes
{
    public string StringProperty { get; set; } = "";
    public IndexSubType SubType { get; set; } = new();
}

[GenerateTypeScriptModel]
public class SecondIndexType
{
    public int IntProperty { get; set; }
}

[GenerateTypeScriptModel("Output/Nested")]
public class NestedIndexType
{
    public string StringProperty { get; set; } = "";
}

public class IndexSubType
{
    public string StringProperty { get; set; } = "";
}
