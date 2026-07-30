using KY.Core;

namespace KY.Generator.Templates.Extensions;

public static class TypeTemplateExtension
{
    /// <summary>
    /// Applies the strict mode of the member to all nullable types nested in its type, e.g. the element type of
    /// <c>List&lt;string?&gt;</c>. Only in strict mode the nullability of a nested type is written
    /// </summary>
    public static void SetStrict(this TypeTemplate? template, bool strict)
    {
        switch (template)
        {
            case NullableTypeTemplate nullableTemplate:
                nullableTemplate.Strict = strict;
                nullableTemplate.Type.SetStrict(strict);
                break;
            case GenericTypeTemplate genericTemplate:
                genericTemplate.Types.ForEach(type => type.SetStrict(strict));
                break;
        }
    }
}
