using KY.Generator;

namespace GeneratorSettingsTest;

/// <summary>
/// Pinned to a class against the preferInterfaces of the configuration. The force applies to this type only, so the
/// referenced ForcedClassDetail still comes out as an interface.
/// </summary>
[GenerateTypeScriptModel]
[GenerateClass(ForceClass = true)]
public class ForcedClassModel
{
    public string Name { get; set; } = "";
    public ForcedClassDetail Detail { get; set; } = new();
}

// Reached through ForcedClassModel, follows the preferInterfaces of the configuration.
public class ForcedClassDetail
{
    public int Number { get; set; }
}
