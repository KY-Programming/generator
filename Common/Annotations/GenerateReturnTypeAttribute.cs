namespace KY.Generator;

/// <summary>
/// Changes the return type of methods
/// </summary>
[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public class GenerateReturnTypeAttribute : Attribute
{
    /// <summary>
    /// Type used for the return of the property/method
    /// </summary>
    public Type? Type { get; }

    /// <summary>
    /// Changes the return type of methods
    /// </summary>
    public GenerateReturnTypeAttribute(Type type)
    {
        this.Type = type;
    }
}
