using System.Collections.Generic;

namespace KY.Generator.Templates
{
    public class AnonymousObjectTemplate : ICodeFragment
    {
        public List<PropertyValueTemplate> Properties { get; }

        public AnonymousObjectTemplate()
        {
            this.Properties = new List<PropertyValueTemplate>();
        }

        public AnonymousObjectTemplate WithProperty(string name, ICodeFragment value)
        {
            this.Properties.Add(new PropertyValueTemplate(name, value));
            return this;
        }
    }
}