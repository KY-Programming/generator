using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using KY.Generator;

namespace Types;

[GenerateTypeScriptModel]
[GenerateStrict]
[GeneratePreferInterfaces]
public class NotNullableStrictInterface
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
public class NotNullableNotStrictInterface
{
    public string StringProperty { get; set; } = string.Empty;
    [Required] public string RequiredStringProperty { get; set; }
    public int IntProperty { get; set; }
    public int? NullableIntProperty { get; set; }
    [Required] public int? RequiredNullableIntProperty { get; set; }
}

[GenerateTypeScriptModel]
[GenerateStrict]
public class NotNullableStrictClass
{
    public string StringProperty { get; set; } = string.Empty;
    [Required] public string RequiredStringProperty { get; set; }
    [Required] public string RequiredStringWithDefaultProperty { get; set; }
    public int IntProperty { get; set; }
    public int? NullableIntProperty { get; set; }
    [Required] public int? RequiredNullableIntProperty { get; set; }
}

[GenerateTypeScriptModel]
public class NotNullableNotStrictClass
{
    public string StringProperty { get; set; } = string.Empty;
    [Required] public string RequiredStringProperty { get; set; }
    public int IntProperty { get; set; }
    public int? NullableIntProperty { get; set; }
    [Required] public int? RequiredNullableIntProperty { get; set; }
}
