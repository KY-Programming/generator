using System;
using System.Collections.Generic;
using KY.Core;
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

            IEnumerable<Person> persons = repository.Get("Id > 3 or Id = 1");
            Console.WriteLine();
            Console.WriteLine("Id  | FirstName  | LastName   | Birthday   | Age | Address");
            foreach (Person readPerson in persons)
            {
                Console.WriteLine($"{readPerson.Id.ToString().PadRight(3)} | {readPerson.FirstName.SubstringSafe(0, 10).PadRight(10)} | {readPerson.LastName.SubstringSafe(0, 10).PadRight(10)} | {readPerson.Birthday.ToShortDateString()} | {readPerson.Age.ToString().PadRight(3)} | {readPerson.Address}");
            }

            Console.WriteLine("Press ENTER to drop the table");
            Console.ReadLine();
            repository.DropTable();
            Console.WriteLine("Table dropped");
            Console.WriteLine("Press ENTER to exit");
            Console.ReadLine();
        }
    }
}
