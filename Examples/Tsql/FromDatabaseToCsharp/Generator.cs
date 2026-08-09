using KY.Generator;

namespace FromDatabaseToCsharp;

public class Generator : GeneratorFluentMain
{
    /// <summary>
    /// The server is not part of the repository - it is started from docker-compose.yml one folder up
    /// before the build, see prepare.js. Encrypt is off because the container uses a self signed
    /// certificate; a real database would not need that.
    /// </summary>
    private const string ConnectionString = "Server=localhost,14330;Database=KyGeneratorExample;User Id=sa;Password=KyGenerator!2026;Encrypt=False";

    public override void Execute()
    {
        this.Read(read => read
                .Tsql(tsql => tsql.UseConnectionString(ConnectionString)
                                  .UseSchema("dbo")
                                  .UseAll()))
            .Write(write => write
                .Reflection(reflection => reflection
                    .Models("Output")));
    }
}
