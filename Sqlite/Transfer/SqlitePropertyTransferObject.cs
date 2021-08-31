using KY.Generator.Sqlite.Helpers;
using KY.Generator.Transfer;

namespace KY.Generator.Sqlite.Transfer
{
    public class SqlitePropertyTransferObject : PropertyTransferObject
    {
        public bool IsNotNull { get; }
        public bool IsPrimaryKey { get; }
        public bool IsAutoIncrement { get; }

        public SqlitePropertyTransferObject(PropertyTransferObject property)
            : base(property)
        {
            this.IsNotNull = this.Attributes.IsNotNull() || !property.Type.IsNullable;
            this.IsPrimaryKey = this.Attributes.IsPrimaryKey();
            this.IsAutoIncrement = this.Attributes.IsAutoIncrement();
        }
    }
}
