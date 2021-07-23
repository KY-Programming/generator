using KY.Generator.Templates;

namespace KY.Generator.TypeScript.Extensions
{
    public static class ParameterTemplateExtension
    {
        public static ParameterTemplate Optional(this ParameterTemplate template)
        {
            template.IsOptional = true;
            return template;
        }
    }
}
