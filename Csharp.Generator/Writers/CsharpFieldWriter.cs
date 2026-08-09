using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.Writers;

namespace KY.Generator.Csharp.Writers;

/// <summary>See <see cref="CsharpPropertyWriter"/> - the same for fields.</summary>
internal class CsharpFieldWriter : FieldWriter
{
    protected override void WriteNullableMarker(TypeTemplate type, IOutputCache output)
    {
        output.If(type.IsNullable).Add("?").EndIf();
    }
}
