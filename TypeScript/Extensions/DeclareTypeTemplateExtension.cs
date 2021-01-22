using KY.Generator.Configurations;
using KY.Generator.TypeScript.Templates;

namespace KY.Generator.TypeScript.Extensions
{
    public static class DeclareTypeTemplateExtension
    {
        public static DeclareTypeTemplate FormatName(this DeclareTypeTemplate declareTypeTemplate, IConfiguration configuration, bool force = false)
        {
            declareTypeTemplate.Name = Formatter.FormatClass(declareTypeTemplate.Name, configuration, force);
            return declareTypeTemplate;
        }

        public static DeclareTypeTemplate Public(this DeclareTypeTemplate template)
        {
            template.IsPublic = true;
            return template;
        }
    }
}