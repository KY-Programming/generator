namespace KY.Generator.Templates.Extensions
{
    public static class AttributableTemplateExtension
    {
        public static AttributeTemplate AddAttribute<T>(this T attributableTemplate, string name)
            where T : AttributeableTempalte
        {
            AttributeTemplate attribute = new(name);
            attributableTemplate.Attributes.Add(attribute);
            return attribute;
        }

        public static T WithAttribute<T>(this T attributableTemplate, string name)
            where T : AttributeableTempalte
        {
            attributableTemplate.AddAttribute(name);
            return attributableTemplate;
        }

        public static AttributeTemplate AddAttribute<T>(this T attributableTemplate, string name, ICodeFragment code)
            where T : AttributeableTempalte
        {
            AttributeTemplate attribute = new(name, code);
            attributableTemplate.Attributes.Add(attribute);
            return attribute;
        }

        public static T WithAttribute<T>(this T attributableTemplate, string name, ICodeFragment code)
            where T : AttributeableTempalte
        {
            attributableTemplate.AddAttribute(name, code);
            return attributableTemplate;
        }
    }
}
