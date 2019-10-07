using System.Collections.Generic;

namespace KY.Generator.Templates
{
    public class GenericTypeTemplate : TypeTemplate
    {
        public List<TypeTemplate> Types { get; }

        public GenericTypeTemplate(string name, string nameSpace = null, bool isNullable = false, bool fromSystem = false)
            : base(name, nameSpace, false, isNullable, fromSystem)
        {
            this.Types = new List<TypeTemplate>();
        }
    }
}