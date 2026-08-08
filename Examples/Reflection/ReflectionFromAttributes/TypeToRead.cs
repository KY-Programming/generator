using KY.Generator;

namespace ReflectionFromAttributes;

/// <summary>
/// The generator does not parse this file - it loads the compiled assembly after the build and looks for
/// types carrying a generate attribute. [GenerateTypeScriptModel] marks this one and passes the output
/// folder, relative to the project directory.
/// </summary>
[GenerateTypeScriptModel("Output")]
public class TypeToRead
{
    public string StringProperty { get; set; } = "";
    public int NumberProperty { get; set; }
}
