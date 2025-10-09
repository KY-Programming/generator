namespace KY.Generator.Angular;

public class AngularOptions(AngularOptions? parent, AngularOptions? global, object? target = null)
    : OptionsBase<AngularOptions>(parent, global, target)
{
    private string? modelOutput;
    private string? serviceOutput;

    public string? ModelOutput
    {
        get => this.GetValue(x => x.modelOutput);
        set => this.modelOutput = value;
    }

    public string? ServiceOutput
    {
        get => this.GetValue(x => x.serviceOutput);
        set => this.serviceOutput = value;
    }
}
