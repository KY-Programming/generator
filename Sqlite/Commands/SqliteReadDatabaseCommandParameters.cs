using KY.Generator.Command;

namespace KY.Generator;

public class SqliteReadDatabaseCommandParameters : GeneratorCommandParameters
{
    public string? ConnectionString { get; set; }
    public bool ReadAll { get; set; }
    public List<string> Tables { get; set; } = new();

    public static string[] Names { get; } = [..ToCommand(nameof(SqliteReadDatabaseCommandParameters)), "sqlite-read"];

    public SqliteReadDatabaseCommandParameters()
        : base(Names.First())
    { }
}
