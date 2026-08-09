using KY.Generator;

namespace SelfReferencing;

// Shared by SelfReferencingAnnotationsNullableEnabled and SelfReferencingAnnotationsNullableDisabled. Both
// projects have to read exactly the same types, otherwise the two nullable modes can not be compared. The
// nullable annotations below are intentional: in the nullable-disabled project they are ignored by the
// compiler (CS8632 is suppressed there), which is exactly the behaviour under test.
[GenerateTypeScriptModel]
public class SelfReferencingType
{
    public string StringProperty { get; set; } = "";
    public SelfReferencingType? SelfProperty { get; set; }
    public List<SelfReferencingType> SelfList { get; set; } = [];
    public Dictionary<string, SelfReferencingType> SelfDictionary { get; set; } = [];
    public CycleA? Cycle { get; set; }
}

/// <summary>
/// Mutual reference A -&gt; B -&gt; A. Neither type can be emitted without the other, so the writer has to
/// break the cycle instead of recursing, and the generated imports must not form an unresolvable loop.
/// </summary>
public class CycleA
{
    public string Name { get; set; } = "";
    public CycleB? B { get; set; }
}

public class CycleB
{
    public string Name { get; set; } = "";
    public CycleA? A { get; set; }
}
