namespace CasingFluent;

/// <summary>The entry type - it pulls both a formatted and an unformatted type into the same run.</summary>
public class MixedCasing
{
    public CaseMe CaseMe { get; set; } = new();
    public KeepMyCase KeepMyCase { get; set; } = new();
}
