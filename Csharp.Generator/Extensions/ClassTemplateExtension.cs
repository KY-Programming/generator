using KY.Generator.Csharp.Templates;
using KY.Generator.Templates;

namespace KY.Generator.Csharp.Extensions
{
    public static class ClassTemplateExtension
    {
        public static UsingTemplate AddUsing(this ClassTemplate classTemplate, string nameSpace)
        {
            UsingTemplate usingTemplate = new UsingTemplate(nameSpace, null, null);
            classTemplate.Usings.Add(usingTemplate);
            return usingTemplate;
        }

        public static ClassTemplate WithUsing(this ClassTemplate classTemplate, string nameSpace)
        {
            classTemplate.AddUsing(nameSpace);
            return classTemplate;
        }

        public static ConstructorTemplate AddConstructor(this ClassTemplate classTemplate)
        {
            ConstructorTemplate constructorTemplate = new ConstructorTemplate(classTemplate);
            classTemplate.Methods.Add(constructorTemplate);
            return constructorTemplate;
        }
    }
}