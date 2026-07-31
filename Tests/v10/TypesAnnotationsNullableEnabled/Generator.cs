using KY.Generator;

namespace Types;

[GenerateTypeScriptModel, GenerateOnlySubTypes]
public class Generator
{
    public Types? Types { get; set; }
}
