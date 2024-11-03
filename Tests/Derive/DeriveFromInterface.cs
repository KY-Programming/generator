using KY.Generator;

namespace Derive;

[GenerateTypeScriptModel]
[GenerateStrict]
public class DeriveFromInterfacePreferClass : IBaseInterface
{
    public string StringProperty { get; set; } = "";
}

[GenerateTypeScriptModel]
[GenerateStrict]
[GeneratePreferInterfaces]
public class DeriveFromInterfacePreferInterface : IBaseInterface
{
    public string StringProperty { get; set; } = "";
}

public interface IBaseInterface
{
    string StringProperty { get; set; }
}
