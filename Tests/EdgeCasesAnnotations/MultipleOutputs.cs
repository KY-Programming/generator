using KY.Generator;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace EdgeCasesAnnotations;

// Three annotated types writing into three different output folders in one build. Two of them expose
// the same sub type, which has to be written into both of their folders rather than shared between
// them, while the third folder stays free of it.

[GenerateTypeScriptModel("Output/MultipleOutputs/First")]
public class FirstType
{
    public string StringProperty { get; set; } = "";
    public MultipleOutputsSubType SubTypeProperty { get; set; } = new();
}

[GenerateTypeScriptModel("Output/MultipleOutputs/Second")]
public class SecondType
{
    public string StringProperty { get; set; } = "";
    public MultipleOutputsSubType SubTypeProperty { get; set; } = new();
}

[GenerateTypeScriptModel("Output/MultipleOutputs/Third")]
public class ThirdType
{
    public string StringProperty { get; set; } = "";
}

/// <summary>Pulled into the first and the second output folder, once by each type that exposes it.</summary>
public class MultipleOutputsSubType
{
    public string StringProperty { get; set; } = "";
}
