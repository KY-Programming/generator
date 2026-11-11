using KY.Generator;

namespace GeneratorSettingsTest;

/// <summary>
/// Prefers classes against the preferInterfaces of the configuration. The preference is passed on, so
/// PreferClassesDetail comes out as a class as well - except ForcedInterfaceDetail, which pins itself to an interface.
/// </summary>
[GenerateTypeScriptModel]
[GeneratePreferClasses]
public class PreferClassesModel
{
    public string Name { get; set; } = "";
    public PreferClassesDetail Detail { get; set; } = new();
    public ForcedInterfaceDetail Forced { get; set; } = new();
}

// Reached through PreferClassesModel, inherits its preference for classes.
public class PreferClassesDetail
{
    public int Number { get; set; }
}

// Reached through PreferClassesModel, but the force wins over the inherited preference.
[GenerateClass(ForceInterface = true)]
public class ForcedInterfaceDetail
{
    public int Number { get; set; }
}
