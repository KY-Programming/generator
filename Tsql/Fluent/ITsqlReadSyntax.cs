namespace KY.Generator;

public interface ITsqlReadSyntax
{
    ITsqlReadSyntax FromTable(string schema, string table);
}
