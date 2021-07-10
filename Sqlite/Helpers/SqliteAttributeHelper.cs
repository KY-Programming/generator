using System.Collections.Generic;
using System.Linq;
using KY.Generator.Transfer;

namespace KY.Generator.Sqlite.Helpers
{
    public static class SqliteAttributeHelper
    {
        public static bool IsNotNull(this IEnumerable<AttributeTransferObject> attributes)
        {
            return attributes.Any(attribute => attribute.Name == nameof(GenerateAsNotNullAttribute));
        }

        public static bool IsAutoIncrement(this IEnumerable<AttributeTransferObject> attributes)
        {
            return attributes.Any(attribute => attribute.Name == nameof(GenerateAsAutoIncrementAttribute));
        }

        public static bool IsPrimaryKey(this IEnumerable<AttributeTransferObject> attributes)
        {
            return attributes.Any(attribute => attribute.Name == nameof(GenerateAsPrimaryKeyAttribute));
        }
    }
}
