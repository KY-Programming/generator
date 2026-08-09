using KY.Generator.Command;

namespace KY.Generator;

public class TsqlReadCommandParameters : GeneratorCommandParameters
{
    public string? ConnectionString { get; set; }
    public string? Schema { get; set; }

    /// <summary>Reads every table of the database, or of <see cref="Schema"/> if one is set.</summary>
    public bool ReadAll { get; set; }

    /// <summary>
    /// Tables to read. An entry can be qualified ("sales.Order") to override <see cref="Schema"/> for that one
    /// table, or a plain name that is resolved against it.
    /// </summary>
    public List<string> Tables { get; set; } = new();

    /// <summary>Single table - the CLI counterpart of <see cref="Tables"/>.</summary>
    public string? Table { get; set; }
    public string? StoredProcedure { get; set; }
    public string? Namespace { get; set; }
    public string? Name { get; set; }

    public static string[] Names { get; } = [..ToCommand(nameof(TsqlReadCommandParameters))];

    public TsqlReadCommandParameters()
        : base(Names.First())
    { }
}
