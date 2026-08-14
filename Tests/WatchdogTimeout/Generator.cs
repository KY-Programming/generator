using KY.Generator;

namespace WatchdogTimeout;

public class Generator : GeneratorFluentMain
{
    /// <summary>Nothing listens here - the port is never bound, so the wait can only run into its timeout.</summary>
    public const string Url = "http://localhost:51988/";

    /// <summary>Short enough to keep the build quick, long enough to be a timeout and not a single failed try.</summary>
    private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(3);

    public override void Execute()
    {
        this.WaitFor(Url)
            .Timeout(Timeout)
            .Read(read => read.Reflection(reflection => reflection.FromType<Model>()))
            .Write(write => write.NoHeader()
                                 .NoIndex()
                                 .TypeScriptModel(model => model.OutputPath("Output")));
    }
}
