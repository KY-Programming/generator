namespace KY.Generator;

[AttributeUsage(AttributeTargets.Assembly)]
public class GenerateWithAttribute : Attribute
{
    public string AssemblyName { get; }
    public string? AssemblyPath { get; }

    public GenerateWithAttribute(string assemblyName, string? assemblyPath = null)
    {
        this.AssemblyName = assemblyName;
        this.AssemblyPath = assemblyPath;
    }
}
