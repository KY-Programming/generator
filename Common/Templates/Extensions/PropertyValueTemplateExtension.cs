namespace KY.Generator.Templates.Extensions
{
    public static class PropertyValueTemplateExtension
    {
        public static PropertyValueTemplate FormatName(this PropertyValueTemplate propertyTemplate, IOptions options, bool force = false)
        {
            propertyTemplate.Name = Formatter.FormatProperty(propertyTemplate.Name, options, force);
            return propertyTemplate;
        }
    }
}
