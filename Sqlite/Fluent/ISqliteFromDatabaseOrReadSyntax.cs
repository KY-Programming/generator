namespace KY.Generator.Sqlite.Fluent
{
    public interface ISqliteFromDatabaseOrReadSyntax : ISqliteFromDatabaseSyntax, ISqliteReadSyntax
    {
        ISqliteFromDatabaseOrReadSyntax UseTable(string tableName);
        ISqliteFromDatabaseOrReadSyntax UseAll();
    }
}
