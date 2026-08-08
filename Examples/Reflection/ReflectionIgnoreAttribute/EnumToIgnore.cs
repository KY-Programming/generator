using KY.Generator;

namespace ReflectionIgnoreAttribute;

/// <summary>
/// Enums are ignored the same way types are.
/// </summary>
[GenerateIgnore]
public enum EnumToIgnore
{
    None,
    Any
}
