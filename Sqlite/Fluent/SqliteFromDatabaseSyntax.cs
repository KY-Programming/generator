namespace KY.Generator;

public class SqliteFromDatabaseSyntax : ISqliteFromDatabaseOrReadSyntax
{
    private readonly SqliteReadSyntax syntax;
    private readonly SqliteReadDatabaseCommandParameters command;

    public SqliteFromDatabaseSyntax(SqliteReadSyntax syntax, string connectionString)
    {
        this.syntax = syntax;
        this.command = new SqliteReadDatabaseCommandParameters
        {
            ConnectionString = connectionString
        };
        this.syntax.Commands.Add(this.command);
    }

    public ISqliteFromDatabaseOrReadSyntax UseTable(string tableName)
    {
        this.command.Tables.Add(tableName);
        return this;
    }

    public ISqliteFromDatabaseOrReadSyntax UseAll()
    {
        this.command.ReadAll = true;
        return this;
    }

    public ISqliteFromDatabaseOrReadSyntax UseConnectionString(string connectionString)
    {
        return this.syntax.UseConnectionString(connectionString);
    }

    public ISqliteFromDatabaseOrReadSyntax UseFile(string file)
    {
        return this.syntax.UseFile(file);
    }
}
