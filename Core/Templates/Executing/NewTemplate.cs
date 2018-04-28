using System.Collections.Generic;
using System.Linq;

namespace KY.Generator.Templates
{
    public class NewTemplate : ChainedCodeFragment
    {
        public override string Separator => " ";
        public TypeTemplate Type { get; }
        public List<CodeFragment> Parameters { get; }

        public NewTemplate(TypeTemplate type, params CodeFragment[] parameters)
            : this(type, (IEnumerable<CodeFragment>)parameters)
        { }

        public NewTemplate(TypeTemplate type, IEnumerable<CodeFragment> parameters)
        {
            this.Type = type;
            this.Parameters = parameters.ToList();
        }
    }
}