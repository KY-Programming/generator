namespace KY.Generator;

public class TypeScriptOptions(TypeScriptOptions? parent, TypeScriptOptions? global, object? target = null)
    : OptionsBase<TypeScriptOptions>(parent, global, target)
{
    private bool? strict;
    private bool? strictFromConfig;
    private bool? noIndex;
    private bool? forceIndex;

    /// <summary>
    /// Generate code that is valid for TypeScripts strict mode. Active by default. Explicitly set values
    /// (<see cref="GenerateNonStrictAttribute"/> or the fluent <c>NonStrict()</c>) win over
    /// <see cref="StrictFromConfig"/>, which is read from the tsconfig.json next to the output.
    /// </summary>
    public bool Strict
    {
        get => this.GetValueOrNull(x => x.strict) ?? this.GetValueOrNull(x => x.strictFromConfig) ?? true;
        set => this.strict = value;
    }

    /// <summary>
    /// The strict mode read from the tsconfig.json of the output folder, or <c>null</c> if there is none. It is
    /// only a fallback: it never overrules an explicitly set <see cref="Strict"/>, no matter on which level.
    /// </summary>
    public bool? StrictFromConfig
    {
        get => this.GetValueOrNull(x => x.strictFromConfig);
        set => this.strictFromConfig = value;
    }

    /// <summary>
    /// Whether a strict mode is set anywhere in this scope or above it, e.g. by the <c>typescript.strict</c> of a
    /// ky-generator.json. If it is, <see cref="StrictFromConfig"/> can never be reached - a narrower scope only ever
    /// sets its own strict mode, which wins as well - so the tsconfig.json does not have to be looked for at all
    /// </summary>
    public bool HasExplicitStrict => this.GetValueOrNull(x => x.strict) != null;

    public bool NoIndex
    {
        get => this.GetValue(x => x.noIndex);
        set => this.noIndex = value;
    }

    public bool ForceIndex
    {
        get => this.GetValue(x => x.forceIndex);
        set => this.forceIndex = value;
    }
}
