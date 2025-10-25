namespace KY.Generator;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public class GenerateCsharpModelAttribute(string relativePath = "", bool onlySubTypes = false)
    : Attribute, IGeneratorCommandAttribute
{
    public string RelativePath { get; } = relativePath;
    public bool OnlySubTypes { get; } = onlySubTypes;

    public IEnumerable<AttributeCommandConfiguration> Commands =>
    [
        new("reflection", this.Parameters)
    ];

    private IEnumerable<string> Parameters
    {
        get
        {
            List<string> parameter =
            [
                "-namespace=$NAMESPACE$",
                "-name=$NAME$",
                $"-language={OutputLanguage.Csharp}"
            ];

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
