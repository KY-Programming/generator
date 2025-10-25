using KY.Core;
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

    public string? RelativePath { get; set; }
    public bool? SkipNamespace { get; set; }
    public bool? PropertiesToFields { get; set; }
    public bool? FieldsToProperties { get; set; }
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
        string baseName = className.TrimEnd("CommandParameters").TrimEnd("Command").ToKebabCase();
        yield return baseName;
        yield return baseName.Replace("'", string.Empty);
    }
}
