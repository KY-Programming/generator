using KY.Generator;

namespace ReflectionReturnTypeAttribute;

[Generate(OutputLanguage.TypeScript, "Output")]
public class Types
{
    public SubType DefaultSubTypeProperty { get; set; }
    
    [GenerateReturnType(typeof(OtherSubType))]
    public SubType ChangedSubTypeProperty { get; set; } 
}
