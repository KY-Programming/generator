﻿using System.ComponentModel;

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
    public string DefaultNullableString { get; set; } = "Default";

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

    // Nullable Types
    public bool? NullableBoolProperty { get; set; }
    public short? NullableShortProperty { get; set; }
    public ushort? NullableUShortProperty { get; set; }
    public int? NullableIntProperty { get; set; }
    public uint? NullableUIntProperty { get; set; }
    public long? NullableLongProperty { get; set; }
    public ulong? NullableULongProperty { get; set; }
    public float? NullableFloatProperty { get; set; }
    public double? NullableDoubleProperty { get; set; }
    public Nullable<bool> Nullable2BoolProperty { get; set; }
    public Nullable<short> Nullable2ShortProperty { get; set; }
    public Nullable<ushort> Nullable2UShortProperty { get; set; }
    public Nullable<int> Nullable2IntProperty { get; set; }
    public Nullable<uint> Nullable2UIntProperty { get; set; }
    public Nullable<long> Nullable2LongProperty { get; set; }
    public Nullable<ulong> Nullable2ULongProperty { get; set; }
    public Nullable<float> Nullable2FloatProperty { get; set; }
    public Nullable<double> Nullable2DoubleProperty { get; set; }

    // System Types
    public string SystemStringProperty { get; set; } = "";
    public short SystemInt16Property { get; set; }
    public int SystemInt32Property { get; set; }
    public long SystemInt64Property { get; set; }
    public float SystemSingleProperty { get; set; }
    public double SystemDoubleProperty { get; set; }
    public DateTime SystemDateTimeProperty { get; set; }
    public Array SystemArrayProperty { get; set; } = Array.Empty<object>();
    public byte SystemByteProperty { get; set; }
    public char SystemCharProperty { get; set; }
    public decimal SystemDecimalProperty { get; set; }
    public Guid SystemGuidProperty { get; set; }
    public object SystemObjectProperty { get; set; } = new();
    public TimeSpan SystemTimeSpanProperty { get; set; }
    public ushort SystemUInt16Property { get; set; }
    public uint SystemUInt32Property { get; set; }
    public ulong SystemUInt64Property { get; set; }

    // Complex Types
    public TypesSubType TypesSubTypeProperty { get; set; } = new();

    // Arrays
    public string[] StringArrayProperty { get; set; } = [];
    public int[] IntArrayProperty { get; set; } = [];
    public byte[] ByteArrayProperty { get; set; } = [];
    public DateTime[] SystemDateTimeArrayProperty { get; set; } = [];
    public TypesSubType[] SubTypeArrayProperty { get; set; } = [];

    // Generics
    public List<string> StringList { get; set; } = [];
    public List<TypesSubType> SubTypeList { get; set; } = [];
    public Dictionary<string, string> StringStringDictionary { get; set; } = new();
    public Dictionary<int, string> IntStringDictionary { get; set; } = new();
    public Dictionary<string, TypesSubType> StringSubTypeDictionary { get; set; } = new();
    public Dictionary<int, TypesSubType> IntSubTypeDictionary { get; set; } = new();
    public Dictionary<TypesSubType, string> SubTypeStringDictionary { get; set; } = new();
    public TypesGenericSubType<string, int> TypesGenericSubType { get; set; } = new();

    // Accessors
    public string ReadonlyProperty => string.Empty;

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

public class TypesSubType
{
    public string Property { get; set; } = "";
}

public class TypesGenericSubType<TOne, TTwo>
{
    public TOne Single { get; }
    public string Single2 { get; }
    public IEnumerable<TOne> Enumerable { get; set; }
    public List<TTwo> List { get; set; }
    public List<string> StringList { get; set; }
}
