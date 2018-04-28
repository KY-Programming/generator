using System.Collections.Generic;

namespace KY.Generator.Templates
{
    public class ExecuteGenericMethodTemplate : ExecuteMethodTemplate
    {
        public List<TypeTemplate> Types { get; }

        public ExecuteGenericMethodTemplate(string name, params TypeTemplate[] types)
            : base(name)
        {
            this.Types = new List<TypeTemplate>(types);
        }
    }
}