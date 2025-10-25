namespace KY.Generator;

public interface ISqliteFromDatabaseOrReadSyntax : ISqliteFromDatabaseSyntax, ISqliteReadSyntax
{
    ISqliteFromDatabaseOrReadSyntax UseTable(string tableName);
    ISqliteFromDatabaseOrReadSyntax UseAll();
}
