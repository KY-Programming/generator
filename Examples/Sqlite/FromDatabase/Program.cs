using System;
using KY.Generator;
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

    public class Generator : GeneratorFluentMain
    {
        public override void Execute()
        {
            this.Read()
                .Sqlite(sqlite => sqlite.UseConnectionString("Data Source=test.db")
                                        .UseTable("test")
                                        .UseAll())
                .Write()
                .ReflectionModels("Output")
                .FieldsToProperties()
                ;
        }
    }
}
