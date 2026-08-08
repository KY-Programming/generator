using KY.Generator;

namespace ReflectionReturnTypeAttribute;

/// <summary>
/// [GenerateProperty(Type = ...)] replaces the type a property is generated with. The C# side keeps
/// working with <see cref="SubType"/>; the generated TypeScript sees <see cref="OtherSubType"/> - useful
/// when a server side type is serialized differently than it is declared.
/// Both types are generated, because both are reachable.
/// </summary>
[GenerateTypeScriptModel("Output")]
public class Types
{
    public SubType DefaultSubTypeProperty { get; set; } = new();

    [GenerateProperty(Type = typeof(OtherSubType))]
    public SubType ChangedSubTypeProperty { get; set; } = new();
}
