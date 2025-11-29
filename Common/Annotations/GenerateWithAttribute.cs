namespace KY.Generator;

/// <summary>
/// Forces the generator to load the specified assembly.
/// </summary>
[AttributeUsage(AttributeTargets.Assembly)]
public class GenerateWithAttribute : Attribute
{
    /// <summary>
    /// The name of the assembly.
    /// Like "KY.Generator.AspDotNet"
    /// </summary>
    public string AssemblyName { get; }

    /// <summary>
    /// The version of the assembly.
    /// Tries to load the assembly with this version. When not found, the newest version will be used.
    /// </summary>
    public string? Version { get; set; }

    /// <summary>
    /// Path to the assembly.
    /// Can be the whole path including the file name or just the directory path.
    /// </summary>
    public string? AssemblyPath { get; set; }

    /// <summary>
    /// The assembly will be loaded with the same version as the assembly containing this attribute.
    /// </summary>
    public bool UseSameVersion { get; set; }

    /// <summary>
    /// The assembly will be loaded with the same version as the generator.
    /// </summary>
    public bool UseGeneratorVersion { get; set; }

    public GenerateWithAttribute(string assemblyName)
    {
        this.AssemblyName = assemblyName;
    }
}
