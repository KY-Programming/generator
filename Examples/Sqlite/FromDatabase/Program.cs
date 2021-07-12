using System;
using Microsoft.Data.Sqlite;

namespace FromDatabase
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            // Create the initial db
            using (SqliteConnection connection = new SqliteConnection("Data Source=test.db"))
            {
                connection.Open();
            }
        }
    }
}
