namespace KY.Generator;

public interface ITsqlReadSyntax : IFluentSyntax
{
    /// <summary>Connects to the database the tables are read from, e.g. "Server=localhost;Database=test;...".</summary>
    ITsqlFromDatabaseOrReadSyntax UseConnectionString(string connectionString);
}
