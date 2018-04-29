namespace KY.Generator.Templates
{
    public class ConstructorTemplate : MethodTemplate
    {
        public ConstructorTemplate(ClassTemplate classTemplate)
            : base(classTemplate, "constructor", null)
        { }
    }
}