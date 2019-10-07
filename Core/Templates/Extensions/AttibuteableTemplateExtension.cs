namespace KY.Generator.Templates.Extensions
{
    public static class AttibuteableTemplateExtension
    {
        public static T WithAttribute<T>(this T attributeableTempalte, string name)
            where T : AttributeableTempalte
        {
            AttributeTemplate attribute = new AttributeTemplate(name);
            attributeableTempalte.Attributes.Add(attribute);
            return attributeableTempalte;
        }
        
        public static T WithAttribute<T>(this T attributeableTempalte, string name, ICodeFragment code)
            where T : AttributeableTempalte
        {
            AttributeTemplate attribute = new AttributeTemplate(name, code);
            attributeableTempalte.Attributes.Add(attribute);
            return attributeableTempalte;
        }
    }
}