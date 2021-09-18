namespace KY.Generator.Tsql.Fluent
{
    public interface ITsqlReadSyntax
    {
        ITsqlReadSyntax FromTable(string schema, string table);
    }
}
