// ReSharper disable All

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace Sqlite.Models
{
    [GeneratedCode("KY.Generator", "8.4.0.0")]
    public partial class ComplexRepository
    {
        private SqliteConnection connection;

        public ComplexRepository(SqliteConnection connection)
        {
            this.connection = connection;
        }

        public void CreateTable()
        {
            using SqliteCommand command = this.connection.CreateCommand();
            command.CommandText = @"
CREATE TABLE IF NOT EXISTS Complex
(
    StringProperty TEXT
);";
            command.ExecuteNonQuery();
        }

        public void DropTable()
        {
            using SqliteCommand command = this.connection.CreateCommand();
            command.CommandText = @"DROP TABLE IF EXISTS Complex;";
            command.ExecuteNonQuery();
        }

        public List<Complex> Get(string filter = null)
        {
            using SqliteCommand command = this.connection.CreateCommand();
            command.CommandText = @"
SELECT StringProperty
FROM Complex";
            List<Complex> result = new List<Complex>();
            if (! string.IsNullOrEmpty(filter))
            {
                command.CommandText += "\nWHERE " + filter;
            }
            using SqliteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Complex entry = new Complex();
                entry.StringProperty = reader.IsDBNull(0) ? null : reader.GetString(0);
                result.Add(entry);
            }
            return result;
        }

        public void Insert(Complex entry)
        {
            using SqliteCommand command = this.connection.CreateCommand();
            command.CommandText = @"INSERT INTO Complex (StringProperty) VALUES (@stringProperty);";
            command.Parameters.Add(new SqliteParameter("@stringProperty", entry.StringProperty ?? (object) DBNull.Value));
            command.ExecuteNonQuery();
        }

        public void Delete(Complex entry)
        {
            using SqliteCommand command = this.connection.CreateCommand();
            command.CommandText = @"
DELETE FROM Complex
WHERE StringProperty = @stringProperty";
            command.Parameters.Add(new SqliteParameter("@stringProperty", entry.StringProperty ?? (object) DBNull.Value));
            command.ExecuteNonQuery();
        }
    }
}

// outputid:6427c6f2-0748-4a76-ab86-205288cc2080
