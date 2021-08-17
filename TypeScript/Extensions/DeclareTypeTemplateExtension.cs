using KY.Generator.TypeScript.Templates;

namespace KY.Generator.TypeScript.Extensions
{
    public static class DeclareTypeTemplateExtension
    {
        public static DeclareTypeTemplate FormatName(this DeclareTypeTemplate declareTypeTemplate, IOptions options, bool force = false)
        {
            declareTypeTemplate.Name = Formatter.FormatClass(declareTypeTemplate.Name, options, force);
            return declareTypeTemplate;
        }

        public static DeclareTypeTemplate Public(this DeclareTypeTemplate template)
        {
            template.IsPublic = true;
            return template;
        }
    }
}
