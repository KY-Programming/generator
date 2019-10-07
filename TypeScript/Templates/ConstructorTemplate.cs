using KY.Generator.Templates;

namespace KY.Generator.TypeScript.Templates
{
    public class ConstructorTemplate : MethodTemplate
    {
        public ConstructorTemplate(ClassTemplate classTemplate)
            : base(classTemplate, "constructor", null)
        { }
    }
}