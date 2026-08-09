namespace KY.Generator.Tsql.Fluent;

public class TsqlFromDatabaseSyntax : ITsqlFromDatabaseOrReadSyntax
{
    private readonly TsqlReadSyntax syntax;
    private readonly TsqlReadCommandParameters command;

    public TsqlFromDatabaseSyntax(TsqlReadSyntax syntax, string connectionString)
    {
        this.syntax = syntax;
        this.command = new TsqlReadCommandParameters
                       {
                           ConnectionString = connectionString
                       };
        this.syntax.Commands.Add(this.command);
    }

    public ITsqlFromDatabaseOrReadSyntax UseSchema(string schema)
    {
        this.command.Schema = schema;
        return this;
    }

    public ITsqlFromDatabaseOrReadSyntax UseTable(string table)
    {
        this.command.Tables.Add(table);
        return this;
    }

    public ITsqlFromDatabaseOrReadSyntax UseTable(string schema, string table)
    {
        // A qualified entry stays independent of the schema set on the command
        this.command.Tables.Add($"{schema}.{table}");
        return this;
    }

    public ITsqlFromDatabaseOrReadSyntax UseAll()
    {
        this.command.ReadAll = true;
        return this;
    }

    public ITsqlFromDatabaseOrReadSyntax UseNamespace(string @namespace)
    {
        this.command.Namespace = @namespace;
        return this;
    }

    public ITsqlFromDatabaseOrReadSyntax UseConnectionString(string connectionString)
    {
        return this.syntax.UseConnectionString(connectionString);
    }
}
