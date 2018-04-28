using System.Collections.Generic;

namespace KY.Generator.Templates
{
    public class ConstraintTemplate : CodeFragment
    {
        public string Name { get; }
        public List<TypeTemplate> Types { get; }

        public ConstraintTemplate(string name, List<TypeTemplate> types)
        {
            this.Name = name;
            this.Types = types;
        }
    }
}