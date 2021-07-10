using System;

namespace KY.Generator.Sqlite.Fluent
{
    public interface ISqliteReadSyntax
    {
        ISqliteFromDatabaseOrReadSyntax UseConnectionString(string connectionString);
        ISqliteFromDatabaseOrReadSyntax UseFile(string file);
    }
}
