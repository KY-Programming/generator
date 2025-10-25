using KY.Generator.Command;

namespace KY.Generator.Sqlite.Commands;

public class SqliteWriteRepositoryCommandParameters : GeneratorCommandParameters
{
    public string? Namespace { get; set; }
    public string? Name { get; set; }
    public string? Table { get; set; }
    public string? ClassName { get; set; }

    public static string[] Names { get; } = [..ToCommand(nameof(SqliteWriteRepositoryCommand)), "sqlite-repository"];

    public SqliteWriteRepositoryCommandParameters()
        : base(Names.First())
    { }
}
