using System.Collections.Generic;

namespace KY.Generator.Templates
{
    public class GenericTypeTemplate : TypeTemplate
    {
        public List<TypeTemplate> Types { get; }

        public GenericTypeTemplate(string name)
            : base(name)
        {
            this.Types = new List<TypeTemplate>();
        }
    }
}