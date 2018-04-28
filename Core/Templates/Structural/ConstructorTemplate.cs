using System.Diagnostics;

namespace KY.Generator.Templates
{
    [DebuggerDisplay("Constructor {Name}")]
    public class ConstructorTemplate : MethodTemplate
    {
        public CodeFragment BaseConstructor { get; set; }
        public CodeFragment ThisConstructor { get; set; }

        public ConstructorTemplate(ClassTemplate classTemplate)
            : base(classTemplate, null, null)
        { }
    }
}