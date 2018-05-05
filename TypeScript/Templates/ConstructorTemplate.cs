namespace KY.Generator.Templates
{
    public class ConstructorTemplate : MethodTemplate
    {
        public CodeFragment[] SuperParameters { get; }

        public ConstructorTemplate(ClassTemplate classTemplate, CodeFragment[] superParameters)
            : base(classTemplate, "constructor", null)
        {
            this.SuperParameters = superParameters;
        }
    }
}