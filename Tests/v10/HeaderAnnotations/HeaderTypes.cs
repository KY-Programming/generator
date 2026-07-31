using KY.Generator;

namespace Header;

/// <summary>
/// The assembly does not set GenerateNoHeader, so the generated file has to start with the KY.Generator
/// auto-generated header. Keep this model tiny - the header is what is under test, not the type mapping.
/// </summary>
[GenerateTypeScriptModel]
public class HeaderTypes
{
    public string StringProperty { get; set; } = "";
    public int IntProperty { get; set; }
}
