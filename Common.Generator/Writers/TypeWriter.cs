using System.Collections.Generic;
using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    public class TypeWriter : ITemplateWriter
    {
        public Dictionary<string, ITypeWriter> Writers { get; }

        public TypeWriter()
        {
            this.Writers = new Dictionary<string, ITypeWriter>();
            this.Writers.Add("Default", new DefaultTypeWriter());
        }

        public virtual void Write(ICodeFragment fragment, IOutputCache output)
        {
            TypeTemplate template = (TypeTemplate)fragment;
            if (this.Writers.ContainsKey(template.Name))
            {
                this.Writers[template.Name].Write(template, output);
            }
            else
            {
                this.Writers["Default"].Write(template, output);
            }
        }
    }
}