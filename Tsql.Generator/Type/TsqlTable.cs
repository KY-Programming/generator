namespace KY.Generator.Tsql.Type;

/// <summary>A table as returned by <see cref="TsqlTypeReader.GetTables"/>.</summary>
public class TsqlTable
{
    public string Schema { get; }
    public string Name { get; }

    public TsqlTable(string schema, string name)
    {
        this.Schema = schema;
        this.Name = name;
    }
}
