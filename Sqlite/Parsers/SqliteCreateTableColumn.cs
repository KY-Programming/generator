namespace KY.Generator.Sqlite.Parsers
{
    public class SqliteCreateTableColumn
    {
        public string Name { get; }
        public string Type { get; }
        public bool Nullable { get; }
        public bool PrimaryKey { get; }
        public bool AutoIncrement { get; }

        public SqliteCreateTableColumn(string name, string type, bool nullable = true, bool primaryKey = false, bool autoIncrement = false)
        {
            this.Name = name;
            this.Type = type;
            this.Nullable = nullable;
            this.PrimaryKey = primaryKey;
            this.AutoIncrement = autoIncrement;
        }
    }
}