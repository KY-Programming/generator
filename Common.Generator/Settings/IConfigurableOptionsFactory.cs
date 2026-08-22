namespace KY.Generator;

/// <summary>
/// An <see cref="IOptionsFactory"/> whose options can be preset from a <c>ky-generator.json</c>. The section is
/// written to the global options - the root of the option tree - so an attribute, the fluent syntax or a CLI
/// parameter still wins over it by sitting closer to the generated type
/// </summary>
public interface IConfigurableOptionsFactory : IOptionsFactory
{
    /// <summary>
    /// The name of the section in the settings file, e.g. <c>typescript</c>
    /// </summary>
    string SettingsSection { get; }

    /// <summary>
    /// What the section is for, written above it by <c>settings-init</c>
    /// </summary>
    string SettingsSectionDescription { get; }

    /// <summary>
    /// The options type this section is written to, e.g. <c>typeof(TypeScriptOptions)</c>
    /// </summary>
    Type OptionsType { get; }

    /// <summary>
    /// Every option the section accepts. A key that is in none of them is reported as a typo
    /// </summary>
    IReadOnlyList<SettingsOption> SettingsOptions { get; }
}
