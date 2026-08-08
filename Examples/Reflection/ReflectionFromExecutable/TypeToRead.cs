using KY.Generator;

namespace ReflectionFromExecutable;

/// <summary>
/// Nothing here differs from a class in a library - the point of this example is the project around it:
/// the assembly the generator reads is an executable (OutputType Exe), not a .dll.
/// </summary>
[GenerateTypeScriptModel("Output")]
public class TypeToRead
{
    public string StringProperty { get; set; } = "";
    public int NumberProperty { get; set; }
}
