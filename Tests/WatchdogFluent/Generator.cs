using KY.Generator;

namespace WatchdogFluent;

public class Generator : GeneratorFluentMain
{
    /// <summary>The endpoint prepare.js starts - see Tests/Shared/Scripts/http-endpoint.js.</summary>
    public const string Url = "http://localhost:51987/";

    public override void Execute()
    {
        // Everything behind the wait is part of the same chain, so it runs once the endpoint answered. The
        // timeout keeps a build from hanging for the default five minutes if the endpoint never came up.
        this.WaitFor(Url)
            .Timeout(TimeSpan.FromSeconds(30))
            .Read(read => read.Reflection(reflection => reflection.FromType<Model>()))
            .Write(write => write.NoHeader()
                                 .NoIndex()
                                 .TypeScriptModel(model => model.OutputPath("Output")));
    }
}
