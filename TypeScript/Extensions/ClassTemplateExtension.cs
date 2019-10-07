using KY.Generator.Templates;
using KY.Generator.TypeScript.Templates;

namespace KY.Generator.TypeScript.Extensions
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

        public static ConstructorTemplate AddConstructor(this ClassTemplate classTemplate)
        {
            ConstructorTemplate template = new ConstructorTemplate(classTemplate);
            classTemplate.Methods.Add(template);
            return template;
        }
    }
}