using KY.Generator.Command;

namespace KY.Generator.Sqlite.Fluent;

public class SqliteReadSyntax : IExecutableSyntax, ISqliteReadSyntax
{
    public List<GeneratorCommandParameters> Commands { get; } = [];

    public ISqliteFromDatabaseOrReadSyntax UseConnectionString(string connectionString)
    {
        return new SqliteFromDatabaseSyntax(this, connectionString);
    }

    public ISqliteFromDatabaseOrReadSyntax UseFile(string file)
    {
        return new SqliteFromDatabaseSyntax(this, $"Data Source={file}");
    }
}
