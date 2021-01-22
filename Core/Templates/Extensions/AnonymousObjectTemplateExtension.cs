namespace KY.Generator.Templates.Extensions
{
    public static class AnonymousObjectTemplateExtension
    {
        public static AnonymousObjectTemplate WithProperty(this AnonymousObjectTemplate template, string name, ICodeFragment value = null)
        {
            template.AddProperty(name, value);
            return template;
        }

        public static PropertyValueTemplate AddProperty(this AnonymousObjectTemplate template, string name, ICodeFragment value = null)
        {
            PropertyValueTemplate propertyValueTemplate = new PropertyValueTemplate(name, value);
            template.Properties.Add(propertyValueTemplate);
            return propertyValueTemplate;
        }
    }
}