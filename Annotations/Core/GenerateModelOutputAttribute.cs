namespace KY.Generator;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Assembly, Inherited = false)]
public class GenerateModelOutputAttribute(string relativePath) : Attribute, IGeneratorCommandAdditionalParameterAttribute
{
    public string RelativePath { get; } = relativePath;

    public IEnumerable<AttributeCommandConfiguration> Commands =>
    [
        new("angular-model", this.Parameters),
        new("angular-service", this.Parameters),
        new("reflection", this.Parameters)
    ];

    private List<string> Parameters
    {
        get
        {
            List<string> parameter = [];
            if (!string.IsNullOrEmpty(this.RelativePath))
            {
                parameter.Add($"-relativeModelPath={this.RelativePath}");
            }
            return parameter;
        }
    }
}
