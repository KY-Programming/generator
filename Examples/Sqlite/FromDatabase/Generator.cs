using KY.Generator;

namespace FromDatabase
{
    public class Generator : GeneratorFluentMain
    {
        public override void Execute()
        {
            this.Read(read => read
                          .Sqlite(sqlite => sqlite.UseConnectionString("Data Source=test.db")
                                                  .UseAll()))
                .Write(write => write
                           .Reflection(reflection => reflection
                                                     .Models("Output")
                                                     .FieldsToProperties())
                );
        }
    }
}
