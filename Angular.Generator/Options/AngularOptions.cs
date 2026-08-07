namespace KY.Generator.Angular;

public class AngularOptions(AngularOptions? parent, AngularOptions? global, object? target = null)
    : OptionsBase<AngularOptions>(parent, global, target)
{
    /// <summary>
    /// Output path used for the generated models, if neither the command nor <see cref="GenerateModelOutputAttribute" /> defines one
    /// </summary>
    public const string DefaultModelOutput = "/ClientApp/src/app/models";

    /// <summary>
    /// Output path used for the generated services, if neither the command nor <see cref="GenerateServiceOutputAttribute" /> defines one
    /// </summary>
    public const string DefaultServiceOutput = "/ClientApp/src/app/services";

    private string? serviceOutput;
    private bool? withSignals;

    public string? ServiceOutput
    {
        get => this.GetValue(x => x.serviceOutput);
        set => this.serviceOutput = value;
    }

    public bool WithSignals
    {
        get => this.GetValue(x => x.withSignals);
        set => this.withSignals = value;
    }
}
