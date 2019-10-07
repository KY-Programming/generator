using System.Diagnostics;

namespace KY.Generator.Templates
{
    [DebuggerDisplay("ExtensionMethod {Name}")]
    public class ExtensionMethodTemplate : MethodTemplate
    {
        public ExtensionMethodTemplate(ClassTemplate classTemplate, string name, TypeTemplate type)
            : base(classTemplate, name, type)
        { }
    }
}