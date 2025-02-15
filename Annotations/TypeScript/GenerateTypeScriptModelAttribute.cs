namespace KY.Generator;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public class GenerateTypeScriptModelAttribute(string relativePath = "", bool onlySubTypes = false)
    : Attribute, IGeneratorCommandAttribute
{
    public string RelativePath { get; } = relativePath;
    public bool OnlySubTypes { get; } = onlySubTypes;

    public IEnumerable<AttributeCommandConfiguration> Commands =>
    [
        new("reflection-read", "-namespace=$NAMESPACE$", "-name=$NAME$"),
        new("typescript-model", this.Parameters)
    ];

    private IEnumerable<string> Parameters
    {
        get
        {
            List<string> parameter = [];
            if (this.RelativePath != string.Empty)
            {
                parameter.Add($"-relativePath={this.RelativePath}");
            }
            if (this.OnlySubTypes)
            {
                parameter.Add("-onlySubTypes");
            }
            return parameter;
        }
    }
}
