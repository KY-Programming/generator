using System.Collections.Generic;
using System.Linq;
using KY.Core;
using KY.Generator.Sqlite.Parsers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KY.Generator.Sqlite.Tests
{
    [TestClass]
    public class SqliteParserTest
    {
        [TestMethod]
        public void CreateTableTest()
        {
            List<ISqliteStatement> statements = SqliteParser.Parse(@"
CREATE TABLE person
(
    Id        INTEGER not null
        constraint test_pk
            primary key autoincrement,
    FirstName TEXT    not null,
    LastName  TEXT    not null,
    Birthday  TEXT,
    age       INTEGER
)
").ToList();
            Assert.AreEqual(1, statements.Count);
            SqliteCreateTable statement = statements.First().CastTo<SqliteCreateTable>();
            Assert.AreEqual(5, statement.Columns.Count);
            Assert.AreEqual("Id", statement.Columns[0].Name);
            Assert.AreEqual("INTEGER", statement.Columns[0].Type);
            Assert.IsTrue(statement.Columns[0].PrimaryKey);
            Assert.IsTrue(statement.Columns[0].AutoIncrement);
            Assert.IsFalse(statement.Columns[0].Nullable);
            Assert.AreEqual("FirstName", statement.Columns[1].Name);
            Assert.AreEqual("TEXT", statement.Columns[1].Type);
            Assert.IsFalse(statement.Columns[1].PrimaryKey);
            Assert.IsFalse(statement.Columns[1].AutoIncrement);
            Assert.IsFalse(statement.Columns[1].Nullable);
            Assert.AreEqual("LastName", statement.Columns[2].Name);
            Assert.AreEqual("TEXT", statement.Columns[2].Type);
            Assert.IsFalse(statement.Columns[2].PrimaryKey);
            Assert.IsFalse(statement.Columns[2].AutoIncrement);
            Assert.IsFalse(statement.Columns[2].Nullable);
            Assert.AreEqual("Birthday", statement.Columns[3].Name);
            Assert.AreEqual("TEXT", statement.Columns[3].Type);
            Assert.IsFalse(statement.Columns[3].PrimaryKey);
            Assert.IsFalse(statement.Columns[3].AutoIncrement);
            Assert.IsTrue(statement.Columns[3].Nullable);
            Assert.AreEqual("age", statement.Columns[4].Name);
            Assert.AreEqual("INTEGER", statement.Columns[4].Type);
            Assert.IsFalse(statement.Columns[4].PrimaryKey);
            Assert.IsFalse(statement.Columns[4].AutoIncrement);
            Assert.IsTrue(statement.Columns[4].Nullable);
        }
    }
}
