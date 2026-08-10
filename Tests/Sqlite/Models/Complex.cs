using KY.Generator;

namespace Sqlite.Models;

/// <summary>A model with a sub model - the sub model gets no column of its own.</summary>
[GenerateSqliteRepository("Output")]
public class Complex
{
    public string StringProperty { get; set; } = "";

    public Simple Simple { get; set; } = new();
}
