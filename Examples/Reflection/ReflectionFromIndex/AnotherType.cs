using KY.Generator;

namespace ReflectionFromIndex;

/// <summary>
/// A second model in the same output folder, so the generated index.ts barrels up more than one export.
/// Fields are read just like properties.
/// </summary>
[GenerateTypeScriptModel("Output")]
public class AnotherType
{
    public string StringField = "";
    public int NumberField;
}
