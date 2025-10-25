using KY.Generator.Syntax;
using KY.Generator.Tsql.Commands;

namespace KY.Generator.Tsql.Fluent;

public class TsqlReadSyntax : ExecutableSyntax, ITsqlReadSyntax
{
    private readonly string connectionString;

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
