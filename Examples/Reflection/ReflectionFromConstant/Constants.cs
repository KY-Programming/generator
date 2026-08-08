using KY.Generator;

namespace ReflectionFromConstant;

/// <summary>
/// Constants and static fields are read with their values and end up as <c>public static readonly</c>
/// members in the generated TypeScript - so a shared set of values only has to be maintained in C#.
/// Instance members would become normal properties instead; this type deliberately has none.
/// </summary>
[GenerateTypeScriptModel("Output")]
public class Constants
{
    public const string StringProperty = "Value-One";
    public const int NumberProperty = 7;

    public static string StaticStringProperty = "Static-Value";
    public static int StaticNumberProperty = 9;
}
