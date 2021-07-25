using KY.Generator;

namespace Sqlite.Models
{
    [GenerateSqliteRepository("Output")]
    public class SimpleWithPrimary
    {
        [GenerateAsPrimaryKey]
        public int Id { get; set; }

        public string StringProperty { get; set; }
    }
}
