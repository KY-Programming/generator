using KY.Generator;

namespace ReflectionReturnTypeAttribute;

[Generate(OutputLanguage.TypeScript, "Output")]
public class Types
{
    public SubType DefaultSubTypeProperty { get; set; }
    
    [GenerateProperty(Type = typeof(OtherSubType))]
    public SubType ChangedSubTypeProperty { get; set; }
}
