using KY.Generator;

namespace Tsql
{
    internal class Generator : GeneratorFluentMain
    {
        public override void Execute()
        {
            this.Read(read => read
                   .Tsql("Server=ky-database;Database=test;User Id=fi49sd;Password=fi49sd", tsql => tsql
                       .FromTable("test", "User")))
               .Write(write => write
                   .Reflection(reflection => reflection.Models("Output")));
        }
    }
}
