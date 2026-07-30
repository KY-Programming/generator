using KY.Generator;

namespace Never;

/// <summary>
/// The DTO is the type that is meant to be generated. It accidentally exposes the internal model, so the generator
/// pulls NeverGeneratedModel into the output as well - which is exactly what GenerateNever has to prevent.
/// </summary>
[GenerateTypeScriptModel]
public class ReferencingDto
{
    public string StringProperty { get; set; } = "";
    public NeverGeneratedModel Model { get; set; } = new();
}

/// <summary>
/// Marked as never generated. Reaching this type aborts the generation with an error that names
/// Output/never-generated-model.ts, so the file (and with it the referencing type) is easy to find.
/// </summary>
[GenerateNever]
public class NeverGeneratedModel
{
    public string StringProperty { get; set; } = "";
}
