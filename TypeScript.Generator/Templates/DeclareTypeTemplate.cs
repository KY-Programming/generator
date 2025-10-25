using System.Collections.Generic;
using KY.Generator.Templates;

namespace KY.Generator.TypeScript.Templates
{
    public class DeclareTypeTemplate : INamespaceChildren
    {
        public string Name { get; set; }
        public List<UsingTemplate> Usings { get; }
        public bool IsPublic { get; set; }
        public ICodeFragment Type { get; }

        public DeclareTypeTemplate(string name, ICodeFragment type)
        {
            this.Name = name;
            this.Type = type;
            this.Usings = new List<UsingTemplate>();
        }
    }
}