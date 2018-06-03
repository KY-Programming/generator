using System.Collections.Generic;

namespace KY.Generator.Templates
{
    public class ClassGenericTemplate : ICodeFragment
    {
        public string Name { get; }
        public List<TypeTemplate> Constraints { get; }

        public ClassGenericTemplate(string name)
        {
            this.Name = name;
            this.Constraints = new List<TypeTemplate>();
        }

        public ConstraintTemplate ToConstraints()
        {
            return new ConstraintTemplate(this.Name, this.Constraints);
        }
    }
}