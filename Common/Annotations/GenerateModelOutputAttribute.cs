namespace KY.Generator;

/// <summary>
/// Changes the output path for all models. The path is relative to the project root.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Assembly, Inherited = false)]
public class GenerateModelOutputAttribute : Attribute
{
    public string RelativePath { get; }

    public GenerateModelOutputAttribute(string relativePath)
    {
        this.RelativePath = relativePath;
    }
}
