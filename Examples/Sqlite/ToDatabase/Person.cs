using System;
using KY.Generator;

namespace ToDatabase
{
    [GenerateSqliteRepository]
    public class Person
    {
        [GenerateAsPrimaryKey]
        [GenerateAsAutoIncrement]
        public int Id { get; set; }

        [GenerateAsNotNull]
        public string FirstName { get; set; }

        [GenerateAsNotNull]
        public string LastName { get; set; }

        public DateTime Birthday { get; set; } = DateTime.Today;
        public int Age { get; set; }
        public string Address { get; set; }
        public Guid Uid { get; set; }
    }
}
