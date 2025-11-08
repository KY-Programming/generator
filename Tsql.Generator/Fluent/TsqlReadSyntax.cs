using KY.Generator.Command;

namespace KY.Generator.Tsql.Fluent;

public class TsqlReadSyntax : IExecutableSyntax, ITsqlReadSyntax
{
    private readonly string connectionString;
    public List<GeneratorCommandParameters> Commands { get; } = [];

    public TsqlReadSyntax(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public ITsqlReadSyntax FromTable(string schema, string table)
    {
        this.Commands.Add(new TsqlReadCommandParameters
        {
            ConnectionString = this.connectionString,
            Schema = schema,
            Table = table
        });
        return this;
    }
}
