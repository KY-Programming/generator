using KY.Generator;

namespace Derive;

[GenerateTypeScriptModel]
[GenerateStrict]
public class DerivedFromAbstractClassPreferClass : AbstractType
{
    public string StringProperty { get; set; } = "";
    public override string AbstractStringProperty { get; set; } = "";
}

[GenerateTypeScriptModel]
[GenerateStrict]
[GeneratePreferInterfaces]
public class DerivedFromAbstractClassPreferInterface : AbstractType
{
    public string StringProperty { get; set; } = "";
    public override string AbstractStringProperty { get; set; } = "";
}

public abstract class AbstractType
{
    public abstract string AbstractStringProperty { get; set; }
}
