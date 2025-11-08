namespace KY.Generator.Statistics;

public class Statistic
{
    public string? Version { get; set; }
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public Guid License { get; set; }
    public bool IsMsBuild { get; set; }
    public bool IsBeforeBuild { get; set; }
    public bool IsSuccess { get; set; }
    public DateTime Start { get; set; }
    public int InitializedModules { get; set; }
    public DateTime InitializationEnd { get; set; }
    public TimeSpan InitializationDuration => this.Start == default || this.InitializationEnd == default ? TimeSpan.Zero : this.InitializationEnd - this.Start;

    public List<CommandStatistic> RanCommands { get; } = new();
    public DateTime RunEnd { get; set; }
    public TimeSpan RunDuration => this.InitializationEnd == default || this.RunEnd == default ? TimeSpan.Zero : this.RunEnd - this.InitializationEnd;

    public long OutputLines { get; set; }
    public DateTime OutputEnd { get; set; }
    public TimeSpan OutputDuration => this.RunEnd == default || this.OutputEnd == default ? TimeSpan.Zero : this.OutputEnd - this.RunEnd;

    public int GeneratedFiles { get; set; }
    public DateTime End { get; set; }
    public TimeSpan Duration => this.Start == default || this.End == default ? TimeSpan.Zero : this.End - this.Start;

    public Dictionary<string, int> CountFilesByLanguage { get; } = new();
    public List<string> Errors { get; } = new();
}
