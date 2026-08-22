using KY.Generator;

namespace GeneratorSettingsTest;

/// <summary>
/// Carries a second type, so the generated file has an import and the quote character from the configuration
/// becomes visible in it.
/// </summary>
[GenerateTypeScriptModel]
public class ConfiguredModel
{
    public string Name { get; set; } = "";
    public ConfiguredDetail Detail { get; set; } = new();
}
