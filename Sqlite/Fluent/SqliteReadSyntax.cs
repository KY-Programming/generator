using KY.Core.Dependency;
using KY.Generator.Syntax;

namespace KY.Generator.Sqlite.Fluent;

public class SqliteReadSyntax : ExecutableSyntax, ISqliteReadSyntax
{
    private readonly IReadFluentSyntaxInternal syntax;

    public IDependencyResolver Resolver => this.syntax.Resolver;

    public SqliteReadSyntax(IReadFluentSyntaxInternal syntax)
    {
        this.syntax = syntax;
    }

    public ISqliteFromDatabaseOrReadSyntax UseConnectionString(string connectionString)
    {
        return new SqliteFromDatabaseSyntax(this, connectionString);
    }

    public ISqliteFromDatabaseOrReadSyntax UseFile(string file)
    {
        return new SqliteFromDatabaseSyntax(this, $"Data Source={file}");
    }
}
