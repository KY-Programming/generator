using KY.Generator;

namespace RunAtSuccess;

/// <summary>One model is enough - what is under test is the hook, not the generation.</summary>
[GenerateTypeScriptModel("Output")]
public class Model
{
    public string StringProperty { get; set; } = "";
}
