using System.Collections.Generic;

namespace KY.Generator.OData.Configurations
{
    public class ODataWriteControllerMethodConfiguration : Codeable
    {
        public string Name { get; set; }
        public List<ODataWriteControllerMethodAttributeConfiguration> Attributes { get; }

        public ODataWriteControllerMethodConfiguration(string name = null)
        {
            this.Name = name;
            this.Attributes = new List<ODataWriteControllerMethodAttributeConfiguration>();
        }

        //public void AddDefaultAttribute(string name, string value = null)
        //{
        //    if (this.Attributes.Any(x => x.Name.Equals(name)))
        //        return;

        //    this.Attributes.Add(new ODataWriteControllerMethodAttributeConfiguration(name).WithProperty(null, value));
        //}

        //public IEnumerable<AttributeTemplate> ToAttributeTemplates()
        //{
        //    foreach (ODataWriteControllerMethodAttributeConfiguration attribute in this.Attributes)
        //    {
        //        StringBuilder builder = new StringBuilder();
        //        foreach (Tuple<string, string> property in attribute.Properties)
        //        {
        //            if (builder.Length > 0)
        //            {
        //                builder.Append(", ");
        //            }
        //            if (string.IsNullOrEmpty(property.Item1))
        //            {
        //                builder.Append($"{property.Item2}");
        //            }
        //            else
        //            {
        //                builder.Append($"{property.Item1} = {property.Item2}");
        //            }
        //        }
        //        ICodeFragment code = builder.Length == 0 ? null : Code.Csharp(builder.ToString());
        //        yield return new AttributeTemplate(attribute.Name, code);
        //    }
        //}
    }
}