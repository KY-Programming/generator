using KY.Generator;

namespace Types;

[GenerateTypeScriptModel]
[GenerateStrict]
public class SelfReferencingType
{
    public string StringProperty { get; set; } = "";
    public SelfReferencingType? SelfProperty { get; set; }
    public List<SelfReferencingType> SelfList { get; set; } = [];
}
