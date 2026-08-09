using KY.Generator;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace EdgeCases;

/// <summary>
/// Covers an annotation on a nested type: the outer type carries none and must stay out of the
/// output, the generator has to find the annotated one inside it. The type is passed on under its
/// source name 'NestedAnnotation.NestedModel' - the CLR spells it 'NestedAnnotation+NestedModel'
/// and the plain 'NestedModel' would not be unique.
/// </summary>
public class NestedAnnotation
{
    [GenerateTypeScriptModel("Output")]
    public class NestedModel
    {
        public string NestedProperty { get; set; } = "";
    }
}
