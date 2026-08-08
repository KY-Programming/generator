using KY.Generator;

namespace ReflectionIgnoreAttribute;

/// <summary>
/// [GenerateIgnore] on a type stops the type itself from being written - which is the point: the
/// TypeScript side of such a type is written by hand once and kept, instead of being overwritten on
/// every run. A member may not just use it, because there would be nothing to import; it is bound to
/// the hand written Output/type-to-ignore.ts with [GenerateImport] on <see cref="TypeToRead"/>.
/// </summary>
[GenerateIgnore]
public class TypeToIgnore
{
    public string StringProperty { get; set; } = "";
}
