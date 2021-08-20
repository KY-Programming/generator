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
    }
}