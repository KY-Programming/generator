namespace KY.Generator
{
    public interface ITsqlReadTableSyntax
    {
        ITsqlReadTableOrSwitchToWriteSyntax FromTable(string schema, string table);
    }
}