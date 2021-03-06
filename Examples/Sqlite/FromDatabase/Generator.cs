﻿using KY.Generator;

namespace FromDatabase
{
    public class Generator : GeneratorFluentMain
    {
        public override void Execute()
        {
            this.Read()
                .Sqlite(sqlite => sqlite.UseConnectionString("Data Source=test.db")
                                        .UseAll())
                .Write()
                .ReflectionModels("Output")
                .FieldsToProperties()
                ;
        }
    }
}