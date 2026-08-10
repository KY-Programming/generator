using KY.Generator;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace EdgeCases;

// GeneratePreferInterfaces as a matrix. Every base kind is generated twice, once with the default
// (the base comes out as a class) and once with the annotation (the base comes out as an interface),
// so the two spellings sit next to each other and can be compared directly. Used elsewhere as
// a single setting, but nowhere else as a pair.

[GenerateTypeScriptModel("Output/PreferInterfaces")]
public class DerivedFromClassPreferClass : BaseClass
{
    public string StringProperty { get; set; } = "";
    public new string NewStringProperty { get; set; } = "";
    public override string VirtualStringProperty { get; set; } = "";
}

[GenerateTypeScriptModel("Output/PreferInterfaces")]
[GeneratePreferInterfaces]
public class DerivedFromClassPreferInterface : BaseClass
{
    public string StringProperty { get; set; } = "";
    public new string NewStringProperty { get; set; } = "";
    public override string VirtualStringProperty { get; set; } = "";
}

/// <summary>Carries a shadowed and a virtual member, so member resolution along the chain is covered in both modes.</summary>
public class BaseClass
{
    public string NewStringProperty { get; set; } = "";
    public virtual string VirtualStringProperty { get; set; } = "";
}

[GenerateTypeScriptModel("Output/PreferInterfaces")]
public class DerivedFromAbstractClassPreferClass : AbstractType
{
    public string StringProperty { get; set; } = "";
    public override string AbstractStringProperty { get; set; } = "";
}

[GenerateTypeScriptModel("Output/PreferInterfaces")]
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

[GenerateTypeScriptModel("Output/PreferInterfaces")]
public class DeriveFromInterfacePreferClass : IBaseInterface
{
    public string StringProperty { get; set; } = "";
}

[GenerateTypeScriptModel("Output/PreferInterfaces")]
[GeneratePreferInterfaces]
public class DeriveFromInterfacePreferInterface : IBaseInterface
{
    public string StringProperty { get; set; } = "";
}

public interface IBaseInterface
{
    string StringProperty { get; set; }
}
