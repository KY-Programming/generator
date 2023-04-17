// ReSharper disable All

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace Sqlite.Models
{
    [GeneratedCode("KY.Generator", "8.6.0.0")]
    public partial class SimpleWithAutoincrementPrimaryRepository
    {
        private SqliteConnection connection;

        public SimpleWithAutoincrementPrimaryRepository(SqliteConnection connection)
        {
            this.connection = connection;
        }

        public void CreateTable()
        {
            using SqliteCommand command = this.connection.CreateCommand();
            command.CommandText = @"
CREATE TABLE IF NOT EXISTS SimpleWithAutoincrementPrimary
(
    Id INTEGER not null primary key autoincrement,
    StringProperty TEXT
);";
            command.ExecuteNonQuery();
        }

        public void DropTable()
        {
            using SqliteCommand command = this.connection.CreateCommand();
            command.CommandText = @"DROP TABLE IF EXISTS SimpleWithAutoincrementPrimary;";
            command.ExecuteNonQuery();
        }

        public List<SimpleWithAutoincrementPrimary> Get(string filter = null)
        {
            using SqliteCommand command = this.connection.CreateCommand();
            command.CommandText = @"
SELECT Id, StringProperty
FROM SimpleWithAutoincrementPrimary";
            List<SimpleWithAutoincrementPrimary> result = new List<SimpleWithAutoincrementPrimary>();
            if (! string.IsNullOrEmpty(filter))
            {
                command.CommandText += "\nWHERE " + filter;
            }
            using SqliteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                SimpleWithAutoincrementPrimary entry = new SimpleWithAutoincrementPrimary();
                entry.Id = reader.GetInt32(0);
                entry.StringProperty = reader.IsDBNull(1) ? null : reader.GetString(1);
                result.Add(entry);
            }
            return result;
        }

        public Int32 Insert(SimpleWithAutoincrementPrimary entry)
        {
            using SqliteCommand command = this.connection.CreateCommand();
            command.CommandText = @"INSERT INTO SimpleWithAutoincrementPrimary (StringProperty) VALUES (@stringProperty);
SELECT last_insert_rowid();";
            command.Parameters.Add(new SqliteParameter("@stringProperty", entry.StringProperty ?? (object) DBNull.Value));
            return (Int32)(long) command.ExecuteScalar();
        }

        public void Update(SimpleWithAutoincrementPrimary entry)
        {
            using SqliteCommand command = this.connection.CreateCommand();
            command.CommandText = @"
UPDATE SimpleWithAutoincrementPrimary
SET StringProperty = @stringProperty
WHERE Id = @id";
            command.Parameters.Add(new SqliteParameter("@id", entry.Id));
            command.Parameters.Add(new SqliteParameter("@stringProperty", entry.StringProperty ?? (object) DBNull.Value));
            command.ExecuteNonQuery();
        }

        public void Delete(SimpleWithAutoincrementPrimary entry)
        {
            using SqliteCommand command = this.connection.CreateCommand();
            command.CommandText = @"
DELETE FROM SimpleWithAutoincrementPrimary
WHERE Id = @id";
            command.Parameters.Add(new SqliteParameter("@id", entry.Id));
            command.ExecuteNonQuery();
        }
    }
}

// outputid:6427c6f2-0748-4a76-ab86-205288cc2080
