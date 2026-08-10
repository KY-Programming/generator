namespace KY.Generator;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = false, AllowMultiple = true)]
public class GenerateCsharpModelAttribute(string relativePath = "", bool onlySubTypes = false)
    : Attribute, IGeneratorCommandAttribute
{
    public string RelativePath { get; } = relativePath;
    public bool OnlySubTypes { get; } = onlySubTypes;

    public IEnumerable<AttributeCommandConfiguration> Commands =>
    [
        new("reflection-read", this.ReadParameters),
        new("reflection-write", this.WriteParameters)
    ];

    private IEnumerable<string> ReadParameters
    {
        get
        {
            List<string> parameter =
            [
                "-namespace=$NAMESPACE$",
                "-name=$NAME$"
            ];
            if (this.OnlySubTypes)
            {
                parameter.Add("-onlySubTypes");
            }
            return parameter;
        }
    }

    private IEnumerable<string> WriteParameters
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
