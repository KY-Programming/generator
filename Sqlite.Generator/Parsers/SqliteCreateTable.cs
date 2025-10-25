using System.Collections.Generic;

namespace KY.Generator.Sqlite.Parsers
{
    public class SqliteCreateTable : ISqliteStatement
    {
        public string TableName { get; set; }
        public List<SqliteCreateTableColumn> Columns { get; set; } = new();

        public SqliteCreateTable(string tableName)
        {
            this.TableName = tableName;
        }
    }
}