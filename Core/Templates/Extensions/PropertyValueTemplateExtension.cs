using KY.Generator.Configurations;

namespace KY.Generator.Templates.Extensions
{
    public static class PropertyValueTemplateExtension
    {
        public static PropertyValueTemplate FormatName(this PropertyValueTemplate propertyTemplate, IConfiguration configuration, bool force = false)
        {
            propertyTemplate.Name = Formatter.FormatProperty(propertyTemplate.Name, configuration, force);
            return propertyTemplate;
        }
    }
}