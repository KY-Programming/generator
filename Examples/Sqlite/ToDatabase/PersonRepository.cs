﻿// ReSharper disable All

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace ToDatabase
{
    [GeneratedCode("KY.Generator", "8.1.0.0")]
    public partial class PersonRepository
    {
        private SqliteConnection connection;

        public PersonRepository(SqliteConnection connection)
        {
            this.connection = connection;
        }

        public void CreateTable()
        {
            using SqliteCommand command = this.connection.CreateCommand();
            command.CommandText = @"
CREATE TABLE IF NOT EXISTS Person
(
    Id INTEGER not null primary key autoincrement,
    FirstName TEXT not null,
    LastName TEXT not null,
    Birthday TEXT not null,
    Age INTEGER not null,
    Address TEXT,
    Uid TEXT not null
);";
            command.ExecuteNonQuery();
        }

        public void DropTable()
        {
            using SqliteCommand command = this.connection.CreateCommand();
            command.CommandText = @"DROP TABLE IF EXISTS Person;";
            command.ExecuteNonQuery();
        }

        public List<Person> Get(string filter = null)
        {
            using SqliteCommand command = this.connection.CreateCommand();
            command.CommandText = @"
SELECT Id, FirstName, LastName, Birthday, Age, Address, Uid
FROM Person";
            List<Person> result = new List<Person>();
            if (! string.IsNullOrEmpty(filter))
            {
                command.CommandText += "\nWHERE " + filter;
            }
            using SqliteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Person entry = new Person();
                entry.Id = reader.GetInt32(0);
                entry.FirstName = reader.GetString(1);
                entry.LastName = reader.GetString(2);
                entry.Birthday = reader.GetDateTime(3);
                entry.Age = reader.GetInt32(4);
                entry.Address = reader.IsDBNull(5) ? null : reader.GetString(5);
                entry.Uid = reader.GetGuid(6);
                result.Add(entry);
            }
            return result;
        }

        public Int32 Insert(Person entry)
        {
            using SqliteCommand command = this.connection.CreateCommand();
            command.CommandText = @"INSERT INTO Person (FirstName, LastName, Birthday, Age, Address, Uid) VALUES (@firstName, @lastName, @birthday, @age, @address, @uid);
SELECT last_insert_rowid();";
            command.Parameters.Add(new SqliteParameter("@firstName", entry.FirstName));
            command.Parameters.Add(new SqliteParameter("@lastName", entry.LastName));
            command.Parameters.Add(new SqliteParameter("@birthday", entry.Birthday));
            command.Parameters.Add(new SqliteParameter("@age", entry.Age));
            command.Parameters.Add(new SqliteParameter("@address", entry.Address ?? (object) DBNull.Value));
            command.Parameters.Add(new SqliteParameter("@uid", entry.Uid.ToString()));
            return (Int32)(long) command.ExecuteScalar();
        }

        public void Update(Person entry)
        {
            using SqliteCommand command = this.connection.CreateCommand();
            command.CommandText = @"
UPDATE Person
SET FirstName = @firstName,
    LastName = @lastName,
    Birthday = @birthday,
    Age = @age,
    Address = @address,
    Uid = @uid
WHERE Id = @id";
            command.Parameters.Add(new SqliteParameter("@id", entry.Id));
            command.Parameters.Add(new SqliteParameter("@firstName", entry.FirstName));
            command.Parameters.Add(new SqliteParameter("@lastName", entry.LastName));
            command.Parameters.Add(new SqliteParameter("@birthday", entry.Birthday));
            command.Parameters.Add(new SqliteParameter("@age", entry.Age));
            command.Parameters.Add(new SqliteParameter("@address", entry.Address ?? (object) DBNull.Value));
            command.Parameters.Add(new SqliteParameter("@uid", entry.Uid));
            command.ExecuteNonQuery();
        }

        public void Delete(Person entry)
        {
            using SqliteCommand command = this.connection.CreateCommand();
            command.CommandText = @"
DELETE FROM Person
WHERE Id = @id";
            command.Parameters.Add(new SqliteParameter("@id", entry.Id));
            command.ExecuteNonQuery();
        }
    }
}

// outputid:b2279010-4764-4c25-8beb-a33bf691dff0
