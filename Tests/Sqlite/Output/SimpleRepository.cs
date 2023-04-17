// ReSharper disable All

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace Sqlite.Models
{
    [GeneratedCode("KY.Generator", "8.6.0.0")]
    public partial class SimpleRepository
    {
        private SqliteConnection connection;

        public SimpleRepository(SqliteConnection connection)
        {
            this.connection = connection;
        }

        public void CreateTable()
        {
            using SqliteCommand command = this.connection.CreateCommand();
            command.CommandText = @"
CREATE TABLE IF NOT EXISTS Simple
(
    StringProperty TEXT,
    NullableStringProperty TEXT not null
);";
            command.ExecuteNonQuery();
        }

        public void DropTable()
        {
            using SqliteCommand command = this.connection.CreateCommand();
            command.CommandText = @"DROP TABLE IF EXISTS Simple;";
            command.ExecuteNonQuery();
        }

        public List<Simple> Get(string filter = null)
        {
            using SqliteCommand command = this.connection.CreateCommand();
            command.CommandText = @"
SELECT StringProperty, NullableStringProperty
FROM Simple";
            List<Simple> result = new List<Simple>();
            if (! string.IsNullOrEmpty(filter))
            {
                command.CommandText += "\nWHERE " + filter;
            }
            using SqliteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Simple entry = new Simple();
                entry.StringProperty = reader.IsDBNull(0) ? null : reader.GetString(0);
                entry.NullableStringProperty = reader.GetString(1);
                result.Add(entry);
            }
            return result;
        }

        public void Insert(Simple entry)
        {
            using SqliteCommand command = this.connection.CreateCommand();
            command.CommandText = @"INSERT INTO Simple (StringProperty, NullableStringProperty) VALUES (@stringProperty, @nullableStringProperty);";
            command.Parameters.Add(new SqliteParameter("@stringProperty", entry.StringProperty ?? (object) DBNull.Value));
            command.Parameters.Add(new SqliteParameter("@nullableStringProperty", entry.NullableStringProperty));
            command.ExecuteNonQuery();
        }

        public void Delete(Simple entry)
        {
            using SqliteCommand command = this.connection.CreateCommand();
            command.CommandText = @"
DELETE FROM Simple
WHERE StringProperty = @stringProperty AND
    NullableStringProperty = @nullableStringProperty";
            command.Parameters.Add(new SqliteParameter("@stringProperty", entry.StringProperty ?? (object) DBNull.Value));
            command.Parameters.Add(new SqliteParameter("@nullableStringProperty", entry.NullableStringProperty));
            command.ExecuteNonQuery();
        }
    }
}

// outputid:6427c6f2-0748-4a76-ab86-205288cc2080
