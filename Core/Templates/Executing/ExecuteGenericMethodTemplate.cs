using System.Collections.Generic;
using System.Linq;

namespace KY.Generator.Templates
{
    public class ExecuteGenericMethodTemplate : ExecuteMethodTemplate
    {
        public List<TypeTemplate> Types { get; }

        public ExecuteGenericMethodTemplate(string name, IEnumerable<TypeTemplate> types = null, params ICodeFragment[] parameters)
            : base(name, parameters)
        {
            this.Types = new List<TypeTemplate>(types ?? Enumerable.Empty<TypeTemplate>());
        }
    }
}