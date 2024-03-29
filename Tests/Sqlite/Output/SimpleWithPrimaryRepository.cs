﻿// ReSharper disable All

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace Sqlite.Models
{
    [GeneratedCode("KY.Generator", "9.0.0.0")]
    public partial class SimpleWithPrimaryRepository
    {
        private SqliteConnection connection;

        public SimpleWithPrimaryRepository(SqliteConnection connection)
        {
            this.connection = connection;
        }

        public void CreateTable()
        {
            using SqliteCommand command = this.connection.CreateCommand();
            command.CommandText = @"
CREATE TABLE IF NOT EXISTS SimpleWithPrimary
(
    Id INTEGER not null primary key,
    StringProperty TEXT
);";
            command.ExecuteNonQuery();
        }

        public void DropTable()
        {
            using SqliteCommand command = this.connection.CreateCommand();
            command.CommandText = @"DROP TABLE IF EXISTS SimpleWithPrimary;";
            command.ExecuteNonQuery();
        }

        public List<SimpleWithPrimary> Get(string filter = null)
        {
            using SqliteCommand command = this.connection.CreateCommand();
            command.CommandText = @"
SELECT Id, StringProperty
FROM SimpleWithPrimary";
            List<SimpleWithPrimary> result = new List<SimpleWithPrimary>();
            if (! string.IsNullOrEmpty(filter))
            {
                command.CommandText += "\nWHERE " + filter;
            }
            using SqliteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                SimpleWithPrimary entry = new SimpleWithPrimary();
                entry.Id = reader.GetInt32(0);
                entry.StringProperty = reader.IsDBNull(1) ? null : reader.GetString(1);
                result.Add(entry);
            }
            return result;
        }

        public void Insert(SimpleWithPrimary entry)
        {
            using SqliteCommand command = this.connection.CreateCommand();
            command.CommandText = @"INSERT INTO SimpleWithPrimary (Id, StringProperty) VALUES (@id, @stringProperty);";
            command.Parameters.Add(new SqliteParameter("@id", entry.Id));
            command.Parameters.Add(new SqliteParameter("@stringProperty", entry.StringProperty ?? (object) DBNull.Value));
            command.ExecuteNonQuery();
        }

        public void Update(SimpleWithPrimary entry)
        {
            using SqliteCommand command = this.connection.CreateCommand();
            command.CommandText = @"
UPDATE SimpleWithPrimary
SET StringProperty = @stringProperty
WHERE Id = @id";
            command.Parameters.Add(new SqliteParameter("@id", entry.Id));
            command.Parameters.Add(new SqliteParameter("@stringProperty", entry.StringProperty ?? (object) DBNull.Value));
            command.ExecuteNonQuery();
        }

        public void Delete(SimpleWithPrimary entry)
        {
            using SqliteCommand command = this.connection.CreateCommand();
            command.CommandText = @"
DELETE FROM SimpleWithPrimary
WHERE Id = @id";
            command.Parameters.Add(new SqliteParameter("@id", entry.Id));
            command.ExecuteNonQuery();
        }
    }
}

// outputid:6427c6f2-0748-4a76-ab86-205288cc2080
