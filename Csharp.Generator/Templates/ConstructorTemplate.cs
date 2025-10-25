using System.Diagnostics;
using KY.Generator.Templates;

namespace KY.Generator.Csharp.Templates
{
    [DebuggerDisplay("Constructor {Name}")]
    public class ConstructorTemplate : MethodTemplate
    {
        public ExecuteMethodTemplate ConstructorCall { get; set; }

        public ConstructorTemplate(ClassTemplate classTemplate)
            : base(classTemplate, classTemplate.Name, null)
        { }
    }
}