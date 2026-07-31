using KY.Generator;

namespace Inheritance;

[GenerateTypeScriptModel]
[GenerateAngularModel("Output/Angular")]
[GenerateOnlySubTypes]
public class Inheritance
{
    public Derived Derived { get; set; }
    public DeriveWithNew DeriveWithNew { get; set; }
    public DerivedFromAbstract DerivedFromAbstract { get; set; }
    public DerivedFromVirtual DerivedFromVirtual { get; set; }
    public Level3 Level3 { get; set; }
    public DerivedFromGeneric DerivedFromGeneric { get; set; }
    public SealedDerived SealedDerived { get; set; }
    public ImplementsInterface ImplementsInterface { get; set; }
}

public class Base
{
    public string StringField;
    public string StringProperty { get; set; }
}

public class Derived : Base
{ }

public class DeriveWithNew : Base
{
    public new string StringField;
    public new int StringProperty { get; set; }
}

public abstract class Abstract
{
    public string StringProperty { get; set; }
    public abstract string AbstractProperty { get; set; }
}

public class DerivedFromAbstract : Abstract
{
    public int IntProperty { get; set; }
    public override string AbstractProperty { get; set; }
}

public class Virtual
{
    public string StringProperty { get; set; }
    public virtual string VirtualProperty { get; set; }
}

public class DerivedFromVirtual : Virtual
{
    public int IntProperty { get; set; }
    public override string VirtualProperty { get; set; }
}

/// <summary>
/// Three level deep chain. Only the most derived type is referenced, so the whole chain has to be pulled in
/// and every level has to end up as its own file with the correct extends clause.
/// </summary>
public class Level1
{
    public string Level1Property { get; set; }
}

public class Level2 : Level1
{
    public string Level2Property { get; set; }
}

public class Level3 : Level2
{
    public string Level3Property { get; set; }
}

/// <summary>
/// Generic base class closed with a concrete type argument by the derived type.
/// </summary>
public class GenericBase<T>
{
    public T GenericProperty { get; set; }
    public string BaseProperty { get; set; }
}

public class DerivedFromGeneric : GenericBase<string>
{
    public int IntProperty { get; set; }
}

public sealed class SealedDerived : Base
{
    public int IntProperty { get; set; }
}

/// <summary>
/// Interface implementation. The interface itself is not annotated, so only the implementing class is
/// generated and it has to carry the interface members itself.
/// </summary>
public interface IHasName
{
    string Name { get; set; }
}

public class ImplementsInterface : IHasName
{
    public string Name { get; set; }
    public int IntProperty { get; set; }
}
