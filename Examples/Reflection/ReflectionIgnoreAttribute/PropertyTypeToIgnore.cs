namespace ReflectionIgnoreAttribute;

/// <summary>
/// Only reachable through an ignored property, so it is never written.
/// </summary>
public class PropertyTypeToIgnore
{
    public string StringProperty { get; set; } = "";
}
