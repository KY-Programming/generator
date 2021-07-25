using KY.Generator;

namespace Sqlite.Models
{
    [GenerateSqliteRepository("Output")]
    public class SimpleWithAutoincrementPrimary
    {
        [GenerateAsPrimaryKey]
        [GenerateAsAutoIncrement]
        public int Id { get; set; }

        public string StringProperty { get; set; }
    }
}
