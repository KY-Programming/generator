namespace KY.Generator.Command;

// TODO: Move all parameters to the command specific parameter classes
public class GeneratorCommandParameters
{
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
}
