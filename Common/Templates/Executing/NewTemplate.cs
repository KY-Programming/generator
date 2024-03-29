using System;
using System.Collections.Generic;
using System.Linq;
using KY.Core;

namespace KY.Generator.Templates
{
    public class NewTemplate : ChainedCodeFragment, ICloneable
    {
        public override string Separator => " ";
        public TypeTemplate Type { get; }
        public List<ICodeFragment> Parameters { get; }

        public NewTemplate(TypeTemplate type, params ICodeFragment[] parameters)
            : this(type, (IEnumerable<ICodeFragment>)parameters)
        { }

        public NewTemplate(TypeTemplate type, IEnumerable<ICodeFragment> parameters)
        {
            this.Type = type;
            this.Parameters = parameters.ToList();
        }

        object ICloneable.Clone()
        {
            return new NewTemplate(this.Type, this.Parameters.Select(x => x.Clone()));
        }
    }
}
