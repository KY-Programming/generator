namespace ReflectionIgnoreAttribute;

/// <summary>
/// Only reachable through an ignored field, so it is never written.
/// </summary>
public class FieldTypeToIgnore
{
    public string StringProperty { get; set; } = "";
}
