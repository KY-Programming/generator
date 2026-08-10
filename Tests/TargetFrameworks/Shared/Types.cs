using KY.Generator;

// The target framework matrix. Every project next to this file reads this one model and differs only in
// what the generator has to be started as, so all of them have to produce the same output apart from the
// outputid - that is the assertion, and it is why the model is shared rather than copied.
//
// Covered is everything that makes the tool start a different build of itself: the .NET versions still in
// support (Net8, Net9, Net10), netstandard2.0 as the oldest target it claims to be able to read, and an
// x86 assembly. net5.0, net6.0 and net7.0 were dropped when the matrix was rebuilt.
//
// Adding a member is a new folder next to this one - nothing here and no other project.md has to change.

namespace TargetFrameworks;

/// <summary>
/// Deliberately small - the exhaustive member matrix lives in the Types projects. This one only needs
/// enough shape to notice a difference between two runs: a primitive, both nullable spellings, a system
/// type, an array, a list, a dictionary, a sub type and a generic sub type.
/// </summary>
[GenerateTypeScriptModel("Output")]
public class Types
{
    public string StringField = "";
    public const string ConstString = "String";

    public string StringProperty { get; set; } = "";
    public int IntProperty { get; set; }
    public bool BoolProperty { get; set; }
    public double DoubleProperty { get; set; }

    public int? NullableIntProperty { get; set; }
    public Nullable<bool> Nullable2BoolProperty { get; set; }

    public DateTime DateTimeProperty { get; set; }
    public Guid GuidProperty { get; set; }
    public decimal DecimalProperty { get; set; }

    public string[] StringArrayProperty { get; set; } = [];
    public List<SubType> SubTypeList { get; set; } = [];
    public Dictionary<string, SubType> SubTypeDictionary { get; set; } = [];

    public SubType SubTypeProperty { get; set; } = new();
    public GenericSubType<string, int> GenericSubType { get; set; } = new();

    public string ReadonlyProperty => string.Empty;
    protected string ProtectedProperty { get; set; } = "";
    private string PrivateProperty { get; set; } = "";
    internal string InternalProperty { get; set; } = "";
}

public class SubType
{
    public string Property { get; set; } = "";
}

public class GenericSubType<TOne, TTwo>
{
    public TOne? Single { get; }
    public IEnumerable<TOne> Enumerable { get; set; } = [];
    public List<TTwo> List { get; set; } = [];
    public List<string> StringList { get; set; } = [];
}
