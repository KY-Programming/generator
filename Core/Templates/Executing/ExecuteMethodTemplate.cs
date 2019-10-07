using System.Collections.Generic;

namespace KY.Generator.Templates
{
    public class ExecuteMethodTemplate : ChainedCodeFragment
    {
        public override string Separator => ".";
        public string Name { get; }
        public List<ICodeFragment> Parameters { get; }

        public ExecuteMethodTemplate(string name, params ICodeFragment[] parameters)
        {
            this.Name = name;
            this.Parameters = new List<ICodeFragment>(parameters);
        }
    }
}