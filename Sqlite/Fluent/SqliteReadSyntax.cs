using KY.Core.Dependency;
using KY.Generator.Command;

namespace KY.Generator;

public class SqliteReadSyntax : IExecutableSyntax, ISqliteReadSyntax
{
    private readonly IReadFluentSyntaxInternal syntax;
    public List<GeneratorCommandParameters> Commands { get; } = [];
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
