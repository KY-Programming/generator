using System.Diagnostics;

namespace KY.Generator.Templates;

[DebuggerDisplay("TypeTemplate: Null")]
public class NullTemplate : TypeTemplate
{
    public NullTemplate()
        : base(string.Empty)
    { }
}