using System;
using Microsoft.Data.Sqlite;

namespace ToDatabase
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("KY.Generator Sqlite Tester");
            Console.WriteLine("==========================");
            Console.WriteLine();

            using SqliteConnection connection = new("Data Source=test.db");
            connection.Open();
            PersonRepository repository = new(connection);
            repository.CreateTable();
            Console.WriteLine("Table created");
            Person person = new() { FirstName = "John", LastName = "Doe" };
            person.Id = repository.Insert(person);
            Console.WriteLine($"Entry {person.Id} added");

            Console.WriteLine("Press ENTER to drop the table");
            Console.ReadLine();
            repository.DropTable();
            Console.WriteLine("Table dropped");
            Console.WriteLine("Press ENTER to exit");
            Console.ReadLine();
        }
    }
}
