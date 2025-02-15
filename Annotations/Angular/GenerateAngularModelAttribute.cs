namespace KY.Generator;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public class GenerateAngularModelAttribute(string relativePath = "")
    : Attribute, IGeneratorCommandAttribute
{
    public string RelativePath { get; } = relativePath;

    public IEnumerable<AttributeCommandConfiguration> Commands =>
    [
        new("reflection-read", "-namespace=$NAMESPACE$", "-name=$NAME$"),
        new("angular-model", this.Parameters)
    ];

    private List<string> Parameters
    {
        get
        {
            List<string> parameter = [];
            if (this.RelativePath != string.Empty)
            {
                parameter.Add($"-relativePath={this.RelativePath}");
            }
            return parameter;
        }
    }
}
