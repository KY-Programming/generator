using System.ComponentModel.DataAnnotations;
using KY.Generator;

namespace Strict;

[GenerateTypeScriptModel]
[GenerateStrict]
[GeneratePreferInterfaces]
public class StrictInterface
{
    public string StringProperty { get; set; } = string.Empty;
    [Required] public string RequiredStringProperty { get; set; }
    [Required] public string RequiredStringWithDefaultProperty { get; set; }
    public int IntProperty { get; set; }
    public int? NullableIntProperty { get; set; }
    [Required] public int? RequiredNullableIntProperty { get; set; }
}

[GenerateTypeScriptModel]
[GeneratePreferInterfaces]
public class NotStrictInterface
{
    public string StringProperty { get; set; } = string.Empty;
    [Required] public string RequiredStringProperty { get; set; }
    public int IntProperty { get; set; }
    public int? NullableIntProperty { get; set; }
    [Required] public int? RequiredNullableIntProperty { get; set; }
}

[GenerateTypeScriptModel]
[GenerateStrict]
public class StrictClass
{
    public string StringProperty { get; set; } = string.Empty;
    [Required] public string RequiredStringProperty { get; set; }
    [Required] public string RequiredStringWithDefaultProperty { get; set; }
    public int IntProperty { get; set; }
    public int? NullableIntProperty { get; set; }
    [Required] public int? RequiredNullableIntProperty { get; set; }
}

[GenerateTypeScriptModel]
public class NotStrictClass
{
    public string StringProperty { get; set; } = string.Empty;
    [Required] public string RequiredStringProperty { get; set; }
    public int IntProperty { get; set; }
    public int? NullableIntProperty { get; set; }
    [Required] public int? RequiredNullableIntProperty { get; set; }
}
