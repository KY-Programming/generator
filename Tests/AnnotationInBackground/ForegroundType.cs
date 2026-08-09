using KY.Generator;

namespace AnnotationInBackground;

/// <summary>
/// The counterpart without <see cref="GenerateInBackgroundAttribute"/>. One annotated type moves the whole
/// project to the background run, so this one has to come out of it as well - it must not be lost on the way.
/// </summary>
[GenerateTypeScriptModel("Output")]
public class ForegroundType
{
    public string StringProperty { get; set; } = "";
}
