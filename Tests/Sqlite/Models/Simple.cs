using KY.Generator;

namespace Sqlite.Models
{
    [GenerateSqliteRepository("Output")]
    public class Simple
    {
        public string StringProperty { get; set; }

        [GenerateAsNotNull]
        public string NullableStringProperty { get; set; }
    }
}
