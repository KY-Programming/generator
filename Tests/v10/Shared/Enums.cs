using KY.Generator;

namespace Enum;

[GenerateTypeScriptModel]
[GenerateOnlySubTypes]
public class Enums
{
    public DefaultEnum DefaultEnum { get; set; }
    public StartingIndexEnum StartingIndexEnum { get; set; }
    public IndexedEnum IndexedEnum { get; set; }
    public SkippedEnum SkippedEnum { get; set; }
    public NegativeEnum NegativeEnum { get; set; }
    public SpecialNameEnum SpecialNameEnum { get; set; }
    public FlagsEnum FlagsEnum { get; set; }
    public SingleValueEnum SingleValueEnum { get; set; }

    // Types
    public ByteEnum ByteEnum { get; set; }
    public SByteEnum SByteEnum { get; set; }
    public ShortEnum ShortEnum { get; set; }
    public UShortEnum UShortEnum { get; set; }
    public IntEnum IntEnum { get; set; }
    public UIntEnum UIntEnum { get; set; }
    public LongEnum LongEnum { get; set; }
    public ULongEnum ULongEnum { get; set; }
}

public enum DefaultEnum
{
    Value1,
    Value2,
    Value3
}

public enum StartingIndexEnum
{
    Value1 = 1,
    Value2,
    Value3
}

public enum IndexedEnum
{
    Value1 = 10,
    Value2 = 20,
    Value3 = 5
}

public enum SkippedEnum
{
    Value1,
    Value3 = 3,
    Value4
}

public enum NegativeEnum
{
    Value1 = -1,
    Value2 = -2,
    Value0 = 0
}

/// <summary>
/// Combinable enum. The combined member is declared as an or-expression of other members, which has to be
/// resolved to its numeric value (3) in the generated output.
/// </summary>
[Flags]
public enum FlagsEnum
{
    None = 0,
    First = 1,
    Second = 2,
    Third = 4,
    FirstAndSecond = First | Second
}

/// <summary>
/// Enum with a single member. Edge case for the value/name mapping objects, which must not degenerate.
/// </summary>
public enum SingleValueEnum
{
    OnlyValue = 42
}

public enum ByteEnum : byte
{
    Value1 = 1,
    Value2 = 2,
    Value3 = 3
}

public enum SByteEnum : sbyte
{
    Value1 = 1,
    Value2 = 2,
    Value3 = 3
}

public enum ShortEnum : short
{
    Value1 = 1,
    Value2 = 2,
    Value3 = 3
}

public enum UShortEnum : ushort
{
    Value1 = 1,
    Value2 = 2,
    Value3 = 3
}

public enum IntEnum : int
{
    Value1 = 1,
    Value2 = 2,
    Value3 = 3
}

public enum UIntEnum : uint
{
    Value1 = 1,
    Value2 = 2,
    Value3 = 3
}

public enum LongEnum : long
{
    Value1 = 1,
    Value2 = 2,
    Value3 = 3
}

public enum ULongEnum : ulong
{
    Value1 = 1,
    Value2 = 2,
    Value3 = 3
}

public enum SpecialNameEnum
{
    Default,
    Number,
    String
}
