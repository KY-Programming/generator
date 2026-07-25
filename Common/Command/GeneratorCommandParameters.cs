using KY.Generator.Extensions;

namespace KY.Generator.Command;

// TODO: Move all parameters to the command specific parameter classes
public class GeneratorCommandParameters
{
    public string CommandName { get; }

    [GeneratorGlobalParameter("onlyAsync")]
    public bool IsOnlyAsync { get; set; }

    [GeneratorGlobalParameter("async")]
    public bool IsAsync { get; set; }

    public bool? IsAsyncAssembly { get; set; }

    [GeneratorGlobalParameter]
    public bool SkipAsyncCheck { get; set; }

    // RelativePath is always relative to the project/output root by KY.Generator convention (attributes
    // like GenerateAngularModel("/ClientApp/...") use a leading slash purely as a readability convention),
    // never an OS-absolute path. Stripping a leading separator here keeps that convention consistent
    // across platforms, since a bare "/" is absolute on Linux/macOS but not on Windows.
    public string? RelativePath
    {
        get;
        set => field = NormalizeRelativePath(value);
    }

    public static string? NormalizeRelativePath(string? path)
    {
        return path?.TrimStart('/', '\\');
    }

    public bool? SkipNamespace { get; set; }
    public bool? PreferInterfaces { get; set; }
    public bool? WithOptionalProperties { get; set; }
    public bool? FormatNames { get; set; }

    // TODO: Execute
    public List<GeneratorCommandParameters> SubCommands { get; } = [];

    public GeneratorCommandParameters(string commandName)
    {
        this.CommandName = commandName;
    }

    protected static IEnumerable<string> ToCommand(string className)
    {
        string baseName = TrimEnd(TrimEnd(className, "CommandParameters"), "Command").ToKebabCase();
        yield return baseName;
        yield return baseName.Replace("-", string.Empty);
    }

    private static string TrimEnd(string value, string trim)
    {
        while (value.EndsWith(trim))
        {
            value = value.Substring(0, value.Length - trim.Length);
        }
        return value;
    }
}
