namespace KY.Generator;

/// <summary>
/// The keys of a <c>ky-generator.json</c> that the generator itself owns. Everything else in the file is a section
/// contributed by an <see cref="IConfigurableOptionsFactory"/>
/// </summary>
public class GeneratorSettings
{
    /// <summary>
    /// The key that names the schema. It is not a setting, editors read it to complete and validate the file
    /// </summary>
    public const string SchemaKey = "$schema";

    /// <summary>
    /// Every own key, described once for the commands that write, validate and document the file. Without this the
    /// same six keys would be spelled out in <c>settings-init</c>, <c>settings-schema</c> and <c>settings-validate</c>
    /// separately, and they would drift apart
    /// </summary>
    public static IReadOnlyList<SettingsOption> Options { get; } =
    [
        SettingsOption.For<GeneratorSettings, bool>("root", false, "Stop the search for further settings files above this one",
                                                    (settings, value) => settings.Root = value, settings => settings.Root),
        SettingsOption.For<GeneratorSettings, bool>("ignoreGlobalSettings", false,
                                                    "Ignore the settings of this machine. The id of the installation is still kept there",
                                                    (settings, value) => settings.IgnoreGlobalSettings = value, settings => settings.IgnoreGlobalSettings),
        // Without a default: pinning the production backend into every settings file only gets in the way the day it
        // moves, and the key is there for a local development backend anyway
        SettingsOption.For<GeneratorSettings, string>("api", null, "The backend the license check talks to",
                                                      (settings, value) => settings.Api = value, settings => settings.Api,
                                                      "https://localhost:5001"),
        SettingsOption.For<GeneratorSettings, bool>("statistics", true, "Set to false to switch the anonymous usage statistics off",
                                                    (settings, value) => settings.Statistics = value, settings => settings.Statistics),
        SettingsOption.For<GeneratorSettings, Guid?>("license", null, "The id of the license to use instead of the id of this installation",
                                                     (settings, value) => settings.License = value, settings => settings.License,
                                                     hidden: true),
        SettingsOption.For<GeneratorSettings, string>("certificate", null, "An offline license certificate, for builds that can not reach the api",
                                                      (settings, value) => settings.Certificate = value, settings => settings.Certificate,
                                                      "<the certificate from https://generator.ky-programming.de/license>")
    ];

    /// <summary>
    /// Stops the search for further settings files above this one. The file that carries it still applies, nothing
    /// above it does. The settings in the application data folder are not part of that search - use
    /// <see cref="IgnoreGlobalSettings"/> for those
    /// </summary>
    public bool Root { get; set; }

    /// <summary>
    /// Skips the settings in the application data folder. Only the files found in the directory tree apply. Rarely
    /// needed - the one case it is built for is a repository whose generated output has to be reproducible on every
    /// machine. The id of the installation is not affected, it is always kept there
    /// </summary>
    public bool IgnoreGlobalSettings { get; set; }

    /// <summary>
    /// The backend the license check and the statistics talk to. Only useful to point a local development build at
    /// a local backend
    /// </summary>
    public string? Api { get; set; }

    /// <summary>
    /// Whether the anonymous usage statistics are sent. Defaults to <c>true</c>
    /// </summary>
    public bool? Statistics { get; set; }

    /// <summary>
    /// The id of the license to use. Set it in a repository whose builds run on a company license. Without it the
    /// id of this installation is used
    /// </summary>
    public Guid? License { get; set; }

    /// <summary>
    /// An offline license certificate, for builds that can not reach the <see cref="Api"/>. Generate one at
    /// https://generator.ky-programming.de/license
    /// </summary>
    public string? Certificate { get; set; }
}
