using KY.Generator;

namespace FromDatabaseToTypeScript;

public class Generator : GeneratorFluentMain
{
    /// <summary>
    /// The same database <c>FromDatabase</c> reads - only the writer differs. See docker-compose.yml and
    /// prepare.js one folder up.
    /// </summary>
    private const string ConnectionString = "Server=localhost,14330;Database=KyGeneratorExample;User Id=sa;Password=KyGenerator!2026;Encrypt=False";

    public override void Execute()
    {
        this.Read(read => read
                      .Tsql(tsql => tsql.UseConnectionString(ConnectionString)
                                        .UseSchema("dbo")
                                        .UseAll()))
            .Write(write => write
                       .TypeScriptModel(model => model
                                            .OutputPath("Output")));
    }
}
