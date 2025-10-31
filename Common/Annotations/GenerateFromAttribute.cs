namespace KY.Generator;

/// <summary>
/// Loads the assembly and generates the code from it.
/// Equal to the load command
/// </summary>
[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
public class GenerateFromAttribute : Attribute, IGeneratorCommandAttribute
{
    public string AssemblyName { get; }

    public GenerateFromAttribute(string assemblyName)
    {
        this.AssemblyName = assemblyName;
    }

    public IEnumerable<AttributeCommandConfiguration> Commands =>
    [
        new("load", $"-assembly=$ASSEMBLYPATH$/{this.AssemblyName}", "-from=$ASSEMBLYLOCATION$"),
        new("annotation")
    ];
}
