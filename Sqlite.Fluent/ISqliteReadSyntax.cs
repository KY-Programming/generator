namespace KY.Generator;

public interface ISqliteReadSyntax : IFluentSyntax
{
    ISqliteFromDatabaseOrReadSyntax UseConnectionString(string connectionString);
    ISqliteFromDatabaseOrReadSyntax UseFile(string file);
}
