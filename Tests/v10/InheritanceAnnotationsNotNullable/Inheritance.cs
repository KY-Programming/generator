using KY.Generator;

namespace Inheritance;

[GenerateTypeScriptModel("Output")]
[GenerateAngularModel("Output/Angular")]
[GenerateOnlySubTypes]
[GenerateStrict]
public class Inheritance
{
    public Derived Derived { get; set; }
    public DeriveWithNew DeriveWithNew { get; set; }
    public DerivedFromAbstract DerivedFromAbstract { get; set; }
    public DerivedFromVirtual DerivedFromVirtual { get; set; }
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
