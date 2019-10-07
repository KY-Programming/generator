using System.Collections.Generic;
using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    public class GenericTypeWriter : ITemplateWriter
    {
        public Dictionary<string, IGenericTypeWriter> Writers { get; }

        public GenericTypeWriter()
        {
            this.Writers = new Dictionary<string, IGenericTypeWriter>();
            this.Writers.Add("Default", new DefaultTypeWriter());
        }

        public virtual void Write(ICodeFragment fragment, IOutputCache output)
        {
            GenericTypeTemplate template = (GenericTypeTemplate)fragment;
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