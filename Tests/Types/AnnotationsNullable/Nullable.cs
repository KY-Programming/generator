using System.ComponentModel.DataAnnotations;
using KY.Generator;

namespace Types;

[GenerateTypeScriptModel]
[GenerateStrict]
[GeneratePreferInterfaces]
public class NullableStrictInterface
{
    public string StringProperty { get; set; } = string.Empty;
    public string? NullableStringProperty { get; set; }

    [Required]
    public string? RequiredNullableStringProperty { get; set; }

    public int IntProperty { get; set; }
    public int? NullableIntProperty { get; set; }

    [Required]
    public int? RequiredNullableIntProperty { get; set; }
}

[GenerateTypeScriptModel]
[GeneratePreferInterfaces]
public class NullableNotStrictInterface
{
    public string StringProperty { get; set; } = string.Empty;
    public string? NullableStringProperty { get; set; }

    [Required]
    public string? RequiredNullableStringProperty { get; set; }

    public int IntProperty { get; set; }
    public int? NullableIntProperty { get; set; }

    [Required]
    public int? RequiredNullableIntProperty { get; set; }
}

[GenerateTypeScriptModel]
[GenerateStrict]
public class NullableStrictClass
{
    public string StringProperty { get; set; } = string.Empty;
    public string? NullableStringProperty { get; set; }

    [Required]
    public string? RequiredNullableStringProperty { get; set; }

    public int IntProperty { get; set; }
    public int? NullableIntProperty { get; set; }

    [Required]
    public int? RequiredNullableIntProperty { get; set; }
}

[GenerateTypeScriptModel]
public class NullableNotStrictClass
{
    public string StringProperty { get; set; } = string.Empty;
    public string? NullableStringProperty { get; set; }

    [Required]
    public string? RequiredNullableStringProperty { get; set; }

    public int IntProperty { get; set; }
    public int? NullableIntProperty { get; set; }

    [Required]
    public int? RequiredNullableIntProperty { get; set; }
}
