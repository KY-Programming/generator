using KY.Generator.Templates;

namespace KY.Generator.Extensions
{
    public static class ClassTemplateExtension
    {
        public static UsingTemplate AddUsing(this ClassTemplate classTemplate, string type, string path)
        {
            UsingTemplate usingTemplate = new UsingTemplate(null, type, path);
            classTemplate.Usings.Add(usingTemplate);
            return usingTemplate;
        }

        public static ClassTemplate WithUsing(this ClassTemplate classTemplate, string type, string path)
        {
            classTemplate.AddUsing(type, path);
            return classTemplate;
        }

        public static ConstructorTemplate AddConstructor(this ClassTemplate classTemplate, params CodeFragment[] superParameters)
        {
            return new ConstructorTemplate(classTemplate, superParameters);
        }
    }
}