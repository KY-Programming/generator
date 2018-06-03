using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KY.Generator.Templates;

namespace KY.Generator.Tsql.OData
{
    public class TsqlODataControllerMethod
    {
        public string Name { get; set; }
        public List<TsqlODataControllerMethodAttribute> Attributes { get; }

        public TsqlODataControllerMethod(string name = null)
        {
            this.Name = name;
            this.Attributes = new List<TsqlODataControllerMethodAttribute>();
        }

        public void AddDefaultAttribute(string name, string value = null)
        {
            if (this.Attributes.Any(x => x.Name.Equals(name)))
                return;

            this.Attributes.Add(new TsqlODataControllerMethodAttribute(name).WithProperty(null, value));
        }

        public IEnumerable<AttributeTemplate> ToAttributeTemplates()
        {
            foreach (TsqlODataControllerMethodAttribute attribute in this.Attributes)
            {
                StringBuilder builder = new StringBuilder();
                foreach (Tuple<string, string> property in attribute.Properties)
                {
                    if (builder.Length > 0)
                    {
                        builder.Append(", ");
                    }
                    if (string.IsNullOrEmpty(property.Item1))
                    {
                        builder.Append($"{property.Item2}");
                    }
                    else
                    {
                        builder.Append($"{property.Item1} = {property.Item2}");
                    }
                }
                ICodeFragment code = builder.Length == 0 ? null : Csharp.Code.Csharp(builder.ToString());
                yield return new AttributeTemplate(attribute.Name, code);
            }
        }
    }
}