using KY.Generator.Command;

namespace KY.Generator.Watchdog.Commands;

public class WatchdogCommandParameters : GeneratorCommandParameters
{
    public string? Url { get; set; }
    public string? LaunchSettings { get; set; }
    public TimeSpan Timeout { get; set; } = TimeSpan.FromMinutes(5);
    public TimeSpan Delay { get; set; } = TimeSpan.FromSeconds(1);
    public TimeSpan Sleep { get; set; } = TimeSpan.FromMilliseconds(100);
    public int Tries { get; set; }
    public string? Command { get; set; }

    public static string[] Names { get; } = [..ToCommand(nameof(WatchdogCommand))];

    public WatchdogCommandParameters()
        : base(Names.First())
    { }
}
