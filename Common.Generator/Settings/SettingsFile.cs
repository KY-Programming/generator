using Newtonsoft.Json.Linq;

namespace KY.Generator;

/// <summary>
/// One <c>ky-generator.json</c> that took part in the resolved settings
/// </summary>
public class SettingsFile(string path, JObject content, bool isRoot = false, bool ignoresGlobalSettings = false,
                          bool isGlobal = false, string? error = null)
{
    public string Path { get; } = path;
    public JObject Content { get; } = content;

    /// <summary>
    /// Set on the file that stopped the search upwards
    /// </summary>
    public bool IsRoot { get; } = isRoot;

    /// <summary>
    /// Set on a file that cuts the settings of the machine off
    /// </summary>
    public bool IgnoresGlobalSettings { get; } = ignoresGlobalSettings;

    /// <summary>
    /// Set on the file in the application data folder
    /// </summary>
    public bool IsGlobal { get; } = isGlobal;

    /// <summary>
    /// Why the file could not be read, or <c>null</c> if it could. A file that does not parse stays in the chain
    /// with no content: it applies nothing, but <c>settings-validate</c> has to be able to report it
    /// </summary>
    public string? Error { get; } = error;

    public override string ToString()
    {
        return this.Path;
    }
}
