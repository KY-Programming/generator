using KY.Generator.Command;

namespace KY.Generator.Tsql.Fluent;

public class TsqlReadSyntax : IExecutableSyntax, ITsqlReadSyntax
{
    public List<GeneratorCommandParameters> Commands { get; } = [];

    public ITsqlFromDatabaseOrReadSyntax UseConnectionString(string connectionString)
    {
        return new TsqlFromDatabaseSyntax(this, connectionString);
    }
}
