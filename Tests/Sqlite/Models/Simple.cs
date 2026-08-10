using KY.Generator;

namespace Sqlite.Models;

[GenerateSqliteRepository("Output")]
public class Simple
{
    public string StringProperty { get; set; } = "";

    /// <summary>Nullable in C#, forced to "not null" in the table by the annotation.</summary>
    [GenerateAsNotNull]
    public string? NullableStringProperty { get; set; }
}
