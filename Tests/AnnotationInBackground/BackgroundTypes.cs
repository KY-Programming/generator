using KY.Generator;

namespace AnnotationInBackground;

/// <summary>
/// The workload of the background pass. The member matrix itself is covered by the Types projects -
/// what matters here is that a type with a sub type, a collection and a dictionary comes out of the
/// background run exactly as it would out of a foreground one.
/// </summary>
[GenerateTypeScriptModel("Output")]
[GenerateInBackground]
public class BackgroundTypes
{
    public string StringProperty { get; set; } = "";
    public int IntProperty { get; set; }
    public DateTime DateTimeProperty { get; set; }
    public SubType SubTypeProperty { get; set; } = new();
    public List<SubType> SubTypeList { get; set; } = [];
    public Dictionary<string, SubType> SubTypeDictionary { get; set; } = [];
}

public class SubType
{
    public string Property { get; set; } = "";
}
