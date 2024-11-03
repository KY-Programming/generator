using KY.Generator;

namespace Derive;

[GenerateTypeScriptModel]
[GenerateStrict]
public class DerivedFromClassPreferClass : BaseClass
{
    public string StringProperty { get; set; } = "";
    public new string NewStringProperty { get; set; } = "";
    public override string VirtualStringProperty { get; set; } = "";
}

[GenerateTypeScriptModel]
[GenerateStrict]
[GeneratePreferInterfaces]
public class DerivedFromClassPreferInterface : BaseClass
{
    public string StringProperty { get; set; } = "";
    public new string NewStringProperty { get; set; } = "";
    public override string VirtualStringProperty { get; set; } = "";
}

public class BaseClass
{
    public string NewStringProperty { get; set; } = "";
    public virtual string VirtualStringProperty { get; set; } = "";
}
