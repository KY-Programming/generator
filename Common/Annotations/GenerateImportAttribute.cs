namespace KY.Generator;

/// <summary>
/// Adds a custom import/using for the given type.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Assembly, Inherited = false, AllowMultiple = true)]
public class GenerateImportAttribute : Attribute
{
    /// <summary>
    /// The type that is replaced by the custom import.
    /// </summary>
    public Type Type { get; }

    /// <summary>
    /// File / module used for the using/import.
    /// </summary>
    public string FileName { get; }

    /// <summary>
    /// Type name used in the using/import.
    /// </summary>
    public string TypeName { get; }

    /// <summary>
    /// Adds a custom import/using for the given type.
    /// </summary>
    /// <param name="type">The type that is replaced by the custom import</param>
    /// <param name="fileName">File / module used for the using/import</param>
    /// <param name="typeName">Type name used in the using/import</param>
    /// <example>
    /// MySpecialWeatherForecastController.cs:
    /// <code>
    /// [GenerateImport(typeof(SpecialWeatherForecast), "@my-lib/models", nameof(SpecialWeatherForecast))]
    /// public class MySpecialWeatherForecastController : ControllerBase {
    /// ...
    /// }
    /// </code>
    /// AssemblyInfo.cs:
    /// <code>
    /// [assembly: GenerateImport(typeof(SpecialWeatherForecast), "@my-lib/models", nameof(SpecialWeatherForecast))]
    /// </code>
    /// </example>
    public GenerateImportAttribute(Type type, string fileName, string typeName)
    {
        this.Type = type;
        this.FileName = fileName;
        this.TypeName = typeName;
    }
}
