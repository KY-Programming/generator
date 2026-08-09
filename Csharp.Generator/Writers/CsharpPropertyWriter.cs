using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.Writers;

namespace KY.Generator.Csharp.Writers;

/// <summary>
/// Adds the "?" a nullable member needs in C#.
/// </summary>
/// <remarks>
/// C# expresses nullability at the place the type is used, not in the type itself - unlike the
/// <c>Nullable&lt;T&gt;</c> a reader may already deliver, which <see cref="CsharpGenericTypeWriter"/> renders. Without
/// this the nullability a reader determined is silently dropped and a column that allows NULL comes out as a plain
/// value type.
/// <para>
/// Only types the mapping marked as nullable get the annotation. That keeps reference types out of it: "string?" in
/// a generated file would need an explicit "#nullable enable", because auto-generated code has no nullable context.
/// </para>
/// </remarks>
internal class CsharpPropertyWriter : PropertyWriter
{
    protected override void WriteNullableMarker(TypeTemplate type, IOutputCache output)
    {
        output.If(type.IsNullable).Add("?").EndIf();
    }
}
