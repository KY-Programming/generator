namespace KY.Generator.Tsql.Configurations;

public class TsqlReadConfiguration
{
    public string? Connection { get; set; }
    public List<TsqlReadEntity> Entities { get; } = new();
    public List<TsqlReadStoredProcedure> StoredProcedures { get; } = new();

    /// <summary>Reads every table of the database, or of <see cref="Schema"/> if one is set.</summary>
    public bool ReadAll { get; set; }

    /// <summary>Schema an entity without its own falls back to. Without it every schema is read.</summary>
    public string? Schema { get; set; }
    public string? Namespace { get; set; }
}
