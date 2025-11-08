using System.Collections.Generic;
using System.Linq;

namespace KY.Generator.Templates
{
    public class ThrowTemplate : ICodeFragment
    {
        public TypeTemplate Type { get; }
        public List<ICodeFragment> Parameters { get; }

        public ThrowTemplate(TypeTemplate type, params ICodeFragment[] parameters)
        {
            this.Type = type;
            this.Parameters = parameters.ToList();
        }
    }
}