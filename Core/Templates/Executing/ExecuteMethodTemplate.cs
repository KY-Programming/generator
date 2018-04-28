using System.Collections.Generic;

namespace KY.Generator.Templates
{
    public class ExecuteMethodTemplate : ChainedCodeFragment
    {
        public override string Separator => ".";
        public string Name { get; }
        public List<CodeFragment> Parameters { get; }

        public ExecuteMethodTemplate(string name, params CodeFragment[] parameters)
        {
            this.Name = name;
            this.Parameters = new List<CodeFragment>(parameters);
        }
    }
}