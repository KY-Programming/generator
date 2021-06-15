using System;
using System.Collections.Generic;
using System.Text;
using KY.Generator;

namespace Tsql
{
    class Generator : GeneratorFluentMain
    {
        public override void Execute()
        {
            this.Read()
                .Tsql("Server=ky-database;Database=test;User Id=fi49sd;Password=fi49sd")
                .FromTable("test", "User")
                .Write()
                .ReflectionModels("Output");
        }
    }
}
