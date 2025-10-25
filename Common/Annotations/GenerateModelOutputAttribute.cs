namespace KY.Generator;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Assembly, Inherited = false)]
public class GenerateModelOutputAttribute(string relativePath) : Attribute, IGeneratorCommandAdditionalParameterAttribute
{
    public string RelativePath { get; } = relativePath;

    // TODO: Rework so that it work with all commands
    public IEnumerable<AttributeCommandConfiguration> Commands =>
    [
        new("angular-model", this.Parameters),
        new("angular-service", this.Parameters),
        new("reflection", this.Parameters),
        new("service-stack-angular-request", this.Parameters)
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
