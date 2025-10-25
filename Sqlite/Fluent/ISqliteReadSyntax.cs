namespace KY.Generator;

public interface ISqliteReadSyntax
{
    ISqliteFromDatabaseOrReadSyntax UseConnectionString(string connectionString);
    ISqliteFromDatabaseOrReadSyntax UseFile(string file);
}
