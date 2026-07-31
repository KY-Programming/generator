using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using KY.Generator;
// ReSharper disable BuiltInTypeReferenceStyle
// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming
// ReSharper disable ConvertNullableToShortForm
// ReSharper disable UnusedType.Global
#pragma warning disable CS0414 // Field is assigned but its value is never used

namespace Types;

public class Types
{
    // Fields
    public string StringField = "";
    public int IntField;
    public DateTime DateTimeField;

    // Constants
    public const string ConstString = "String";
    public const short ConstShort = 1;
    public const ushort ConstUShort = 2;
    public const int ConstInt = 3;
    public const uint ConstUInt = 4;
    public const long ConstLong = 5;
    public const ulong ConstULong = 6;
    public const float ConstFloat = 7.1f;
    public const double ConstDouble = 8.2;
    public const bool ConstBool = true;
    public const byte ConstByte = 9;
    public const sbyte ConstSByte = 10;
    public const NumericEnum ConstNumericEnum = NumericEnum.One;

    // Default Values
    [DefaultValue("Default")]
    public string DefaultString { get; set; } = "Default";

    [DefaultValue(1)]
    public short DefaultShort { get; set; } = 1;

    [DefaultValue(2)]
    public ushort DefaultUShort { get; set; } = 2;

    [DefaultValue(3)]
    public int DefaultInt { get; set; } = 3;

    [DefaultValue(4)]
    public uint DefaultUInt { get; set; } = 4;

    [DefaultValue(5)]
    public long DefaultLong { get; set; } = 5;

    [DefaultValue(6)]
    public ulong DefaultULong { get; set; } = 6;

    [DefaultValue(7.1f)]
    public float DefaultFloat { get; set; } = 7.1f;

    [DefaultValue(8.2)]
    public double DefaultDouble { get; set; } = 8.2;

    [DefaultValue(true)]
    public bool DefaultBool { get; set; } = true;

    // Default Nullable Values
    [DefaultValue("Default")]
    public string? DefaultNullableString { get; set; } = "Default";

    [DefaultValue(1)]
    public short? DefaultNullableShort { get; set; } = 1;

    [DefaultValue(2)]
    public ushort? DefaultNullableUShort { get; set; } = 2;

    [DefaultValue(3)]
    public int? DefaultNullableInt { get; set; } = 3;

    [DefaultValue(4)]
    public uint? DefaultNullableUInt { get; set; } = 4;

    [DefaultValue(5)]
    public long? DefaultNullableLong { get; set; } = 5;

    [DefaultValue(6)]
    public ulong? DefaultNullableULong { get; set; } = 6;

    [DefaultValue(7.1f)]
    public float? DefaultNullableFloat { get; set; } = 7.1f;

    [DefaultValue(8.2)]
    public double? DefaultNullableDouble { get; set; } = 8.2;

    [DefaultValue(true)]
    public bool? DefaultNullableBool { get; set; } = true;
    
    [DefaultValue(NumericEnum.One)]
    public NumericEnum DefaultNullableNumericEnum { get; set; } = NumericEnum.One;

    // Required Properties
    public required string RequiredStringProperty { get; set; } = "";
    public required short RequiredShortProperty { get; set; }
    public required ushort RequiredUShortProperty { get; set; }
    public required int RequiredIntProperty { get; set; }
    public required uint RequiredUIntProperty { get; set; }
    public required long RequiredLongProperty { get; set; }
    public required ulong RequiredULongProperty { get; set; }
    public required float RequiredFloatProperty { get; set; }
    public required double RequiredDoubleProperty { get; set; }
    public required bool RequiredBoolProperty { get; set; }
    public required object RequiredObjectProperty { get; set; } = new();
    public required byte RequiredByteProperty { get; set; }
    public required sbyte RequiredSByteProperty { get; set; }
    public required NumericEnum RequiredNumericEnumProperty { get; set; }

    // Require Attribute Properties
    [Required]
    public string RequiredAttributeStringProperty { get; set; } = "";

    [Required]
    public short RequiredAttributeShortProperty { get; set; }

    [Required]
    public ushort RequiredAttributeUShortProperty { get; set; }

    [Required]
    public int RequiredAttributeIntProperty { get; set; }

    public uint RequiredAttributeUIntProperty { get; set; }

    [Required]
    public long RequiredAttributeLongProperty { get; set; }

    [Required]
    public ulong RequiredAttributeULongProperty { get; set; }

    [Required]
    public float RequiredAttributeFloatProperty { get; set; }

    [Required]
    public double RequiredAttributeDoubleProperty { get; set; }

    [Required]
    public bool RequiredAttributeBoolProperty { get; set; }

    [Required]
    public object RequiredAttributeObjectProperty { get; set; } = new();

    [Required]
    public byte RequiredAttributeByteProperty { get; set; }

    [Required]
    public sbyte RequiredAttributeSByteProperty { get; set; }
    
    [Required]
    public NumericEnum RequiredAttributeNumericEnumProperty { get; set; }

    // Types
    public string StringProperty { get; set; } = "";
    public short ShortProperty { get; set; }
    public ushort UShortProperty { get; set; }
    public int IntProperty { get; set; }
    public uint UIntProperty { get; set; }
    public long LongProperty { get; set; }
    public ulong ULongProperty { get; set; }
    public float FloatProperty { get; set; }
    public double DoubleProperty { get; set; }
    public bool BoolProperty { get; set; }
    public object ObjectProperty { get; set; } = new();
    public byte ByteProperty { get; set; }
    public sbyte SByteProperty { get; set; }
    public NumericEnum NumericEnumProperty { get; set; }

    // Nullable Types
    public string? NullableStringProperty { get; set; }
    public bool? NullableBoolProperty { get; set; }
    public short? NullableShortProperty { get; set; }
    public ushort? NullableUShortProperty { get; set; }
    public int? NullableIntProperty { get; set; }
    public uint? NullableUIntProperty { get; set; }
    public long? NullableLongProperty { get; set; }
    public ulong? NullableULongProperty { get; set; }
    public float? NullableFloatProperty { get; set; }
    public double? NullableDoubleProperty { get; set; }
    public NumericEnum? NullableNumericEnumProperty { get; set; }
    public Nullable<bool> Nullable2BoolProperty { get; set; }
    public Nullable<short> Nullable2ShortProperty { get; set; }
    public Nullable<ushort> Nullable2UShortProperty { get; set; }
    public Nullable<int> Nullable2IntProperty { get; set; }
    public Nullable<uint> Nullable2UIntProperty { get; set; }
    public Nullable<long> Nullable2LongProperty { get; set; }
    public Nullable<ulong> Nullable2ULongProperty { get; set; }
    public Nullable<float> Nullable2FloatProperty { get; set; }
    public Nullable<double> Nullable2DoubleProperty { get; set; }
    public Nullable<NumericEnum> Nullable2NumericEnumProperty { get; set; }

    // System Types
    public String SystemStringProperty { get; set; } = "";
    public Int16 SystemInt16Property { get; set; }
    public Int32 SystemInt32Property { get; set; }
    public Int64 SystemInt64Property { get; set; }
    public Single SystemSingleProperty { get; set; }
    public Double SystemDoubleProperty { get; set; }
    public DateTime SystemDateTimeProperty { get; set; }
    public Array SystemArrayProperty { get; set; } = Array.Empty<object>();
    public Byte SystemByteProperty { get; set; }
    public SByte SystemSByteProperty { get; set; }
    public Char SystemCharProperty { get; set; }
    public Decimal SystemDecimalProperty { get; set; }
    public Guid SystemGuidProperty { get; set; }
    public Object SystemObjectProperty { get; set; } = new();
    public TimeSpan SystemTimeSpanProperty { get; set; }
    public UInt16 SystemUInt16Property { get; set; }
    public UInt32 SystemUInt32Property { get; set; }
    public UInt64 SystemUInt64Property { get; set; }

    // System types added after the classic DateTime/Guid/TimeSpan set above, each plain, nullable and
    // inside a collection. Decimal and Guid are already covered further up.
    public DateOnly SystemDateOnlyProperty { get; set; }
    public TimeOnly SystemTimeOnlyProperty { get; set; }
    public DateTimeOffset SystemDateTimeOffsetProperty { get; set; }
    public Uri SystemUriProperty { get; set; } = new("https://example.com");
    public Version SystemVersionProperty { get; set; } = new(1, 0);
    public DateOnly? NullableSystemDateOnlyProperty { get; set; }
    public TimeOnly? NullableSystemTimeOnlyProperty { get; set; }
    public DateTimeOffset? NullableSystemDateTimeOffsetProperty { get; set; }
    public Uri? NullableSystemUriProperty { get; set; }
    public List<DateOnly> SystemDateOnlyList { get; set; } = [];
    public Dictionary<string, DateTimeOffset> SystemDateTimeOffsetDictionary { get; set; } = [];

    // Complex Types
    public SubType SubTypeProperty { get; set; } = new();

    // Records and value types
    public PositionalRecord PositionalRecord { get; set; } = new("", 0);
    public PropertyRecord PropertyRecord { get; set; } = new();
    public DerivedRecord DerivedRecord { get; set; } = new("", 0);
    public RecordStruct RecordStruct { get; set; }
    public PlainStruct PlainStruct { get; set; }
    public PlainStruct? NullableStruct { get; set; }
    public ReadOnlyStruct ReadOnlyStruct { get; set; }
    public InitOnlyType InitOnlyType { get; set; } = new() { RequiredInitProperty = "" };

    // Arrays
    public string[] StringArrayProperty { get; set; } = [];
    public int[] IntArrayProperty { get; set; } = [];
    public byte[] ByteArrayProperty { get; set; } = [];
    public DateTime[] SystemDateTimeArrayProperty { get; set; } = [];
    public SubType[] SubTypeArrayProperty { get; set; } = [];

    // Nullable collections. Two independent axes: the collection itself and its element type.
    public List<string>? NullableList { get; set; }
    public string[]? NullableArray { get; set; }
    public IEnumerable<string>? NullableEnumerable { get; set; }
    public Dictionary<string, string>? NullableDictionary { get; set; }
    public List<string?> ListOfNullable { get; set; } = [];
    public string?[] ArrayOfNullable { get; set; } = [];
    public IEnumerable<string?> EnumerableOfNullable { get; set; } = [];
    public Dictionary<string, string?> DictionaryOfNullableValue { get; set; } = [];
    public List<string?>? NullableListOfNullable { get; set; }
    public List<int?> ListOfNullableInt { get; set; } = [];
    public List<int>? NullableListOfInt { get; set; }
    public List<SubType?> ListOfNullableSubType { get; set; } = [];
    public List<SubType>? NullableListOfSubType { get; set; }
    public Dictionary<string, SubType?> DictionaryOfNullableSubType { get; set; } = [];
    public List<List<string>>? NullableNestedList { get; set; }
    public List<List<string>?> NestedNullableList { get; set; } = [];

    // Generics
    public List<string> StringList { get; set; } = [];
    public List<SubType> SubTypeList { get; set; } = [];
    public IList<string> StringIList { get; set; } = [];
    public IList<SubType> SubTypeIList { get; set; } = [];
    public IEnumerable<string> StringIEnumerable { get; set; } = [];
    public IEnumerable<SubType> SubTypeIEnumerable { get; set; } = [];
    public IReadOnlyList<string> StringIReadOnlyList { get; set; } = [];
    public IReadOnlyList<SubType> SubTypeIReadOnlyList { get; set; } = [];
    public ICollection<string> StringICollection { get; set; } = [];
    public ICollection<SubType> SubTypeICollection { get; set; } = [];
    public IReadOnlyCollection<string> StringIReadOnlyCollection { get; set; } = [];
    public IReadOnlyCollection<SubType> SubTypeIReadOnlyCollection { get; set; } = [];
    public Dictionary<string, string> StringStringDictionary { get; set; } = [];
    public Dictionary<int, string> IntStringDictionary { get; set; } = [];
    public Dictionary<string, SubType> StringSubTypeDictionary { get; set; } = [];
    public Dictionary<int, SubType> IntSubTypeDictionary { get; set; } = [];
    public Dictionary<SubType, string> SubTypeStringDictionary { get; set; } = [];
    public IDictionary<string, string> StringStringIDictionary { get; set; } = new Dictionary<string, string>();
    public IReadOnlyDictionary<string, string> StringStringIReadOnlyDictionary { get; set; } = new Dictionary<string, string>();
    public GenericSubType<string, int> GenericSubType { get; set; } = new();

    // Accessors
    public string ReadonlyProperty => string.Empty; 

    // ReSharper disable once ValueParameterNotUsed
    public string WriteonlyProperty
    {
        set { }
    }

    protected string ProtectedProperty { get; set; } = "";
    private string PrivateProperty { get; set; } = "";
    internal string InternalProperty { get; set; } = "";
    protected string ProtectedField = "";
    private string PrivateField = "";
    internal string InternalField = "";
}

public class SubType
{
    public string Property { get; set; } = "";
}

public class GenericSubType<TOne, TTwo>
{
    public TOne Single { get; }
    public string Single2 { get; }
    public IEnumerable<TOne> Enumerable { get; set; }
    public List<TTwo> List { get; set; }
    public List<GenericSubType<TOne, TTwo>> GenericList { get; set; }
    public List<string> StringList { get; set; }
}

public enum NumericEnum
{
    Zero = 0,
    One = 1,
    Two = 2,
    Three = 3
}

/// <summary>
/// Positional record - the primary constructor parameters become init-only properties.
/// </summary>
public record PositionalRecord(string Name, int Value);

/// <summary>
/// Record declared with a classic property body.
/// </summary>
public record PropertyRecord
{
    public string Name { get; set; } = "";
    public int Value { get; init; }
}

public record DerivedRecord(string Name, int Value) : PositionalRecord(Name, Value)
{
    public bool Extra { get; init; }
}

public record struct RecordStruct(string Name, int Value);

public struct PlainStruct
{
    public string Name { get; set; }
    public int Value { get; set; }
}

public readonly struct ReadOnlyStruct
{
    public string Name { get; init; }
    public int Value { get; init; }
}

/// <summary>
/// init-only and required members on a plain class.
/// </summary>
public class InitOnlyType
{
    public required string RequiredInitProperty { get; init; }
    public string InitProperty { get; init; } = "";
    public string GetSetProperty { get; set; } = "";
}
