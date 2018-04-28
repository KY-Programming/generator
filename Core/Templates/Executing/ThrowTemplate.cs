using System.Collections.Generic;
using System.Linq;

namespace KY.Generator.Templates
{
    public class ThrowTemplate : CodeFragment
    {
        public TypeTemplate Type { get; }
        public List<CodeFragment> Parameters { get; }

        public ThrowTemplate(TypeTemplate type, params CodeFragment[] parameters)
        {
            this.Type = type;
            this.Parameters = parameters.ToList();
        }
    }
}